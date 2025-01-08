using System.Collections.Concurrent;
using System.ComponentModel;
using client.globals;
using Grpc.Core;
using Grpc.Net.Client;
using gRpcProtos;
using LLibrary.Guards;
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
                    Label l = new Label
                    {
                        Text = $"""
                            ({preloadMessageListObject.UserId})
                            {preloadMessageListObject.Context}
                            """,
                        AutoSize = true,
                        ForeColor =
                            preloadMessageListObject.UserId == _userId ? Color.Brown : Color.Black,
                    };
                    MessagePanel.Controls.Add(l);
                }

                MessageStreaming();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
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
                    Invoke(() =>
                    {
                        Label l = new Label
                        {
                            Text = $@"({response.AuthorUserId}) {response.Context}",
                            AutoSize = true,
                            ForeColor =
                                response.AuthorUserId == _userId ? Color.Brown : Color.Black,
                        };
                        MessagePanel.Controls.Add(l);
                    });
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
            _messages.TryAdd(MessageTextBox.Text);
            MessageTextBox.Clear();
        }
    }

}
