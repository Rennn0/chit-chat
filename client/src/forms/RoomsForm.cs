using client.globals;
using client.Properties;
using client.src.custom;
using Grpc.Net.Client;
using gRpcProtos;
using LLibrary.Guards;

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
            this.Text += $@" - {LocalSettings.Default["Token"]}";
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prompt = Prompts.DoubleInputPrompt(
                "room name ...",
                "room description",
                SendCreateRoomRequest
            );

            prompt.ShowDialog();
        }

        private void joinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prompt = Prompts.SingleInputForm("room id", JoinRoomCallback);
            prompt.ShowDialog();
        }

        private void JoinRoomCallback(string roomId)
        {
            ChatForm cf = new ChatForm(roomId, LocalSettings.Default["Token"]);
            cf.Show();
        }

        // TODO es rame uketess ver izavs??
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private async void SendCreateRoomRequest(string name, string description)
        {
            try
            {
                GrpcChannel channel = Globals.GetGrpcChannel();
                MessageExchangeService.MessageExchangeServiceClient client = new(channel);

                CreateRoomResponse? res = await client.CreateRoomAsync(
                    new CreateRoomRequest()
                    {
                        Description = description,
                        HostUserId = LocalSettings.Default["Token"],
                        Name = name,
                    }
                );

                MessageBox.Show(@$"Created new room _ {res.RoomId}");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
