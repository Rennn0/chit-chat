using System.ComponentModel;
using System.Runtime.CompilerServices;
using client.Properties;
using client.src.forms;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using gRpcProtos;
using LLibrary.Guards;
using Message = gRpcProtos.Message;

namespace client.forms
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
            _channel = GrpcChannel.ForAddress(
                Guard.AgainstNull(Properties.Resources.MessageServerUrl)
            );
            _client = new MessageExchangeService.MessageExchangeServiceClient(_channel);
            this.Icon = Resources.ButterflyIcon;
        }

        private readonly GrpcChannel _channel;
        private readonly MessageExchangeService.MessageExchangeServiceClient _client;

        private AsyncDuplexStreamingCall<Message, Message>? _streamingCall;
        private Thread? _messageThread;

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += $@" - {Settings.Default.token}";
        }

        private void TopLayout_Paint(object sender, PaintEventArgs e) { }

        private void listOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hi u clieck ctrl l");
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hi u clieck ctrl alt c");
        }

        private void joinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hi u clieck ctrl j");
        }
    }
}
