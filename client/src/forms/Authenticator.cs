using generator;

namespace client.forms
{
    public partial class Authenticator : Form
    {
        public Authenticator()
        {
            InitializeComponent();
        }

        private async void Authenticator_Load(object sender, EventArgs e)
        {
            try
            {
                await Task.Delay(1000);
                this.Hide();

                Form next = string.IsNullOrWhiteSpace(RuntimeTrexSettings.Get(TrexSettings.Token))
                    ? new AuthorizationForm()
                    : new RoomsForm();

                next.Closing += Next_Closing;
                next.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Next_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
    }
}
