using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public event PropertyChangedEventHandler? PropertyChanged;

        public ChatForm()
        {
            InitializeComponent();
            _channel = GrpcChannel.ForAddress(
                Guard.AgainstNull(Properties.Resources.MessageServerUrl)
            );
            _client = new MessageExchangeService.MessageExchangeServiceClient(_channel);
        }

        protected void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void CreateAccClicked(object sender, EventArgs e) { }

        //    CreateUserResponse call = await _client.CreateUserAsync(
        //        new CreateUserRequest { Email = Email, Username = Username }
        //    );

        //    _userId = call.UserId;
        //    UserLabel.Text += _userId;
        //}

        private void CreateRoomClicked(object sender, EventArgs e)
        {
            //CreateRoomResponse call = await _client.CreateRoomAsync(
            //    new CreateRoomRequest { Name = RoomName }
            //);
            //RoomId = call.RoomId;
            //RoomLabel.Text += RoomId;
        }

        private void SendClicked(object sender, EventArgs e)
        {
            _streamingCall ??= _client.MessageStreaming();
            _streamingCall.RequestStream.WriteAsync(
                new Message
                {
                    Context = SendTextBox.Text,
                    //RoomId = RoomId,
                    //UserId = _userId,
                    Timestamp = DateTime.UtcNow.ToTimestamp(),
                }
            );

            if (_messageThread is not null)
            {
                return;
            }
            _messageThread = new Thread(ReadMessageStream) { IsBackground = true };
            _messageThread.Start();
        }

        private async void ReadMessageStream()
        {
            Guard.AgainstNull(_streamingCall);
            await foreach (Message? response in _streamingCall.ResponseStream.ReadAllAsync())
            {
                //ReceivedText += response.Context + Environment.NewLine;
                //SendText = string.Empty;
            }
        }

        private void ExistingRoomBox_TextChanged(object sender, EventArgs e) { }

        private readonly GrpcChannel _channel;
        private readonly MessageExchangeService.MessageExchangeServiceClient _client;

        private AsyncDuplexStreamingCall<Message, Message>? _streamingCall;
        private Thread? _messageThread;

        private void MainForm_Load(object sender, EventArgs e) { }

        private void mainFormBindingsBindingSource_CurrentChanged(object sender, EventArgs e) { }
    }
}
