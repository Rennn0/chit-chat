using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using client.Properties;

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
            await Task.Delay(1000);
            this.Hide();

            Form next = new AuthorizationForm(); /*string.IsNullOrEmpty(Settings.Default.token)
                ? new AuthorizationForm()
                : new ChatForm();*/

            next.Closing += Next_Closing;
            next.Show();
        }

        private void Next_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
    }
}
