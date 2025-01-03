using client.Properties;
using generator;

namespace client.forms
{
    public partial class RoomsForm : Form
    {
        public RoomsForm()
        {
            InitializeComponent();

            this.Icon = Resources.ButterflyIcon;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += $@" - {RuntimeTrexSettings.Get(TrexSettings.Token)}";
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hi u clieck ctrl alt c");
        }

        private void joinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hi u clieck ctrl j");
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
