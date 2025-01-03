using client.globals;
using Grpc.Net.Client;
using gRpcProtos;

namespace client.controls
{
    public partial class ChatControl : UserControl
    {
        private readonly string _roomId;
        private readonly string _userId;

        public ChatControl(string roomId, string userId)
        {
            InitializeComponent();
            _roomId = roomId;
            _userId = userId;
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

                foreach (PreloadMessageListObject preloadMessageListObject in response.Messages)
                {
                    Label l = new Label { Text = preloadMessageListObject.Context };
                    MessagePanel.Controls.Add(l);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
