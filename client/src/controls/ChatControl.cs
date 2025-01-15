using System.Collections.Concurrent;
using System.Text;
using client.globals;
using client.network;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using gRpcProtos;
using LLibrary.Guards;
using LLibrary.Logging;
using Message = gRpcProtos.Message;

namespace client.controls
{
    public partial class ChatControl : UserControl
    {
        private readonly string _roomId;
        private readonly string _userId;
        private readonly BlockingCollection<string> _messages;
        private AsyncDuplexStreamingCall<Message, Message>? _call;

        public ChatControl(string roomId, string userId)
        {
            InitializeComponent();
            _roomId = roomId;
            _userId = userId;
            _messages = [];
            this.Load += ChatControl_Load;
            MessagePanel.FlowDirection = FlowDirection.TopDown;
            MessagePanel.WrapContents = false;
            MessagePanel.AutoScroll = true;
            MessageTextBox.KeyDown += MessageTextBox_KeyDown;
            MessagePanel.Padding = new Padding(15, 10, 15, 0);
        }

        private void MessageTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendButton_Click(this, EventArgs.Empty);
            }
        }

        private async void ChatControl_Load(object? sender, EventArgs e)
        {
            try
            {
                GrpcChannel channel = Globals.GetGrpcChannel();
                MessageExchangeService.MessageExchangeServiceClient client = new(channel);

                PreloadMessageResponse? response = await client.PreloadMessagesAsync(
                    new PreloadMessagesRequest { RoomId = _roomId, UserId = _userId }
                );

                foreach (
                    PreloadMessageListObject preloadMessageListObject in response.Messages.OrderBy(
                        r => r.Timestamp
                    )
                )
                {
                    AddLabel(preloadMessageListObject);
                }

                MessageStreaming();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddLabel(PreloadMessageListObject obj) =>
            AddLabel(obj.Context, obj.UserId, obj.Timestamp);

        private void AddLabel(Message obj) =>
            AddLabel(obj.Context, obj.AuthorUserId, obj.Timestamp);

        private void AddLabel(string context, string userId, Timestamp timestamp)
        {
            ToolTip tt =
                new()
                {
                    IsBalloon = false,
                    ShowAlways = true,
                    ToolTipIcon = ToolTipIcon.Info,
                    ToolTipTitle = "Message details",
                };

            Label l =
                new()
                {
                    Text = $@"{context}",
                    AutoSize = true,
                    ForeColor = userId == _userId ? Color.Brown : Color.Black,
                    Font = new Font("Arial", 18),
                    BorderStyle = BorderStyle.Fixed3D,
                    Padding = new Padding(5),
                    Margin = new Padding(3, 3, 3, 10),
                };

            StringBuilder sb = new();
            sb.AppendLine($"Sender {userId}");
            sb.AppendLine($"Timestamp {timestamp.ToDateTimeOffset().ToLocalTime()}");
            tt.SetToolTip(l, sb.ToString());
            MessagePanel.Controls.Add(l);
        }

        private void MessageStreaming()
        {
            try
            {
                GrpcChannel channel = Globals.GetGrpcChannel();
                MessageExchangeService.MessageExchangeServiceClient client = new(channel);
                _call = client.MessageStreaming();

                new Thread(MessageWriterThread).Start();
                new Thread(MessageReaderThread).Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async void MessageReaderThread()
        {
            try
            {
                Guard.AgainstNull(_call);
                await foreach (Message response in _call.ResponseStream.ReadAllAsync())
                {
                    Invoke(() => AddLabel(response));
                }
            }
            catch (Exception exception)
            {
                Invoke(() => MessageBox.Show(exception.Message));
            }
        }

        private async void MessageWriterThread()
        {
            try
            {
                Guard.AgainstNull(_call);
                await _call.RequestStream.WriteAsync(
                    new Message
                    {
                        AuthorUserId = _userId,
                        RoomId = _roomId,
                        Context = string.Empty,
                    }
                );
                foreach (string message in _messages.GetConsumingEnumerable())
                {
                    await _call.RequestStream.WriteAsync(
                        new Message
                        {
                            AuthorUserId = _userId,
                            Context = message,
                            RoomId = _roomId,
                        }
                    );
                }
            }
            catch (Exception exception)
            {
                Invoke(() => MessageBox.Show(exception.Message));
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            _messages.TryAdd(MessageTextBox.Text.TrimStart(['\r', '\n']));
            MessageTextBox.Clear();
        }

        // TODO filebis gacvlis feature
        private async void attachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog fileDialog = new();
                fileDialog.Multiselect = true;
                fileDialog.CheckFileExists = true;

                if (fileDialog.ShowDialog() != DialogResult.OK)
                    return;
                int bytesWritten = await FileServer.SendFileAsync(fileDialog.FileNames);
                MessageBox.Show($@"Sent {bytesWritten} bytes");
            }
            catch (Exception ex)
            {
                Diagnostics.LOG_ERROR(ex.Message);
            }
        }
    }
}
