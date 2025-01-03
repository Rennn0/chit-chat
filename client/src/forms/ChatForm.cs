namespace client.forms
{
    public partial class ChatForm : Form
    {
        private readonly string _roomId;
        private readonly string _userId;

        public ChatForm(string roomId, string userId)
        {
            _roomId = roomId;
            _userId = userId;

            InitializeComponent();
            this.Load += ChatForm_Load;
        }

        private void ChatForm_Load(object? sender, EventArgs e)
        {
            this.Text = $@"Room {_roomId}";
        }
    }
}
