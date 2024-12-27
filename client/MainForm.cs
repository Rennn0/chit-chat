using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using gRpcProtos;
using Message = gRpcProtos.Message;

namespace client
{
    public partial class MainForm : Form
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainForm()
        {
            InitializeComponent();

            _channel = GrpcChannel.ForAddress("https://localhost:7293/");
            _client = new MessageExchangeService.MessageExchangeServiceClient(_channel);
        }

        protected void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async void CreateAccClicked(object sender, EventArgs e) { }

        //    CreateUserResponse call = await _client.CreateUserAsync(
        //        new CreateUserRequest { Email = Email, Username = Username }
        //    );

        //    _userId = call.UserId;
        //    UserLabel.Text += _userId;
        //}

        private async void CreateRoomClicked(object sender, EventArgs e)
        {
            MainFormBindings binding = mainFormBindingsBindingSource.DataSource as MainFormBindings;

            binding.Text += "ZOROOOO";

            //CreateRoomResponse call = await _client.CreateRoomAsync(
            //    new CreateRoomRequest { Name = RoomName }
            //);
            //RoomId = call.RoomId;
            //RoomLabel.Text += RoomId;
        }

        private void SendClicked(object sender, EventArgs e)
        {
            //_streamingCall ??= _client.MessageStreaming();
            //_streamingCall.RequestStream.WriteAsync(
            //    new Message
            //    {
            //        Context = SendTextBox.Text,
            //        RoomId = RoomId,
            //        UserId = _userId,
            //        Timestamp = DateTime.UtcNow.ToTimestamp(),
            //    }
            //);

            //if (_messageThread is null)
            //{
            //    _messageThread = new Thread(async () =>
            //    {
            //        await foreach (
            //            Message? response in _streamingCall.ResponseStream.ReadAllAsync()
            //        )
            //        {
            //            ReceivedText += response.Context + Environment.NewLine;
            //            SendText = string.Empty;
            //        }
            //    })
            //    {
            //        IsBackground = true,
            //    };
            //    _messageThread.Start();
            //}
        }

        private void ExistingRoomBox_TextChanged(object sender, EventArgs e) { }

        private readonly GrpcChannel _channel;
        private readonly MessageExchangeService.MessageExchangeServiceClient _client;

        private AsyncDuplexStreamingCall<Message, Message>? _streamingCall;
        private Thread? _messageThread;

        private void MainForm_Load(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(this);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) { }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e) { }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e) { }

        private void toolTip1_Popup(object sender, PopupEventArgs e) { }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e) { }
    }
}
