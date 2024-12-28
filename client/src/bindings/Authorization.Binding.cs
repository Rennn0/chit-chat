namespace client.bindings
{
    public class AuthorizationBinding : BindingBase
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _userId = string.Empty;
        private char _passwordChar = '*';
        private bool _passwordChecked;

        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public string UserId
        {
            get => _userId;
            set => SetField(ref _userId, value);
        }

        public char PasswordChar
        {
            get => _passwordChar;
            set => SetField(ref _passwordChar, value);
        }

        public bool PasswordChecked
        {
            get => _passwordChecked;
            set
            {
                SetField(ref _passwordChecked, value);
                PasswordChar = value ? '\0' : '*';
            }
        }
    }
}
