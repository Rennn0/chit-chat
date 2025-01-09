using client.globals;
using LLibrary.Guards;
using llibrary.Http;

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
                Globals.Key = await HttpHandlers.Sync();

                if (string.IsNullOrEmpty(Globals.Key))
                {
                    MessageBox.Show(@"Server is unavailable");
                    Application.Exit();
                }
                // TODO logout
                //                Encryption.FlushOnDisk(
                //                    @"
                //MessageServerUrl=http://20.199.82.7:5000
                //RabbitHost=20.199.82.7
                //RabbitUsername=luka
                //RabbitPassword=danelia
                //RabbitPort=5672
                //RabbitRoomExchange=rooms
                //DumpFile=threaddump.txt
                //Token= ",
                //                    Globals.Key,
                //                    ".trex"
                //                );

                int affected = LocalSettings.Init(Globals.Key);
                if (affected == 0)
                {
                    MessageBox.Show(@"Corrupted or deleted settings");
                    Application.Exit();
                }

                Form next = string.IsNullOrWhiteSpace(LocalSettings.Default["Token"])
                    ? new AuthorizationForm()
                    : new RoomsForm();

                next.Closing += Next_Closing;

                this.Hide();
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
