using client.bindings;
using client.globals;
using client.Properties;
using Grpc.Net.Client;
using gRpcProtos;
using llibrary.Guards;

namespace client.forms
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
            this.Icon = Resources.ButterflyIcon;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void AuthorizationForm_Load(object sender, EventArgs e)
        {
        }

        private void AutoFillButton_Click(object sender, EventArgs e)
        {
            AuthorizationBinding bind = Guard.AgainstNull(
                authorizationBindingBindingSource.Current as AuthorizationBinding
            );
            string[] randomUsernames = Resources.RandomUsernames.Split(',');
            Random r = new();
            byte[] b = new byte[16];
            r.NextBytes(b);

            bind.G_username =
                randomUsernames[r.Next(0, randomUsernames.Length)].Replace("\"", string.Empty)
                + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            bind.G_password = Convert.ToHexString(b);
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            AuthorizationBinding bind = Guard.AgainstNull(
                authorizationBindingBindingSource.Current as AuthorizationBinding
            );

            GrpcChannel channel = Globals.GetGrpcChannel();

            MessageExchangeService.MessageExchangeServiceClient client = new(channel);

            CreateUserResponse response = Guard.AgainstNull(
                client.CreateUser(
                    new CreateUserRequest { Password = bind.G_password, Username = bind.G_username }
                )
            );

            if (response.Code == CreateUserResponse.Types.CODE.UsernameUsed)
            {
                AutoFillButton_Click(this, EventArgs.Empty);
                return;
            }

            if (!LocalSettings.UpdateOrCreate("Token", response.UserId))
            {
                throw new InvalidOperationException();
            }

            this.Hide();
            RoomsForm cf = new();
            cf.Closing += (s, a) => this.Dispose();
            cf.Show();
        }
    }
}