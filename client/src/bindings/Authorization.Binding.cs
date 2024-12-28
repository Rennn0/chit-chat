namespace client.bindings
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ObservableAttribute : System.Attribute
    {
        public ObservableAttribute() { }
    }

    public partial class AuthorizationBinding : BindingBase
    {
        [Observable]
        private string _username = string.Empty;

        [Observable]
        private string _password = string.Empty;

        [Observable]
        private string _userId = string.Empty;

        [Observable]
        private char _passwordChar = '*';

        private bool _passwordChecked;

        public bool PasswordChecked
        {
            get => _passwordChecked;
            set
            {
                SetField(ref _passwordChecked, value);
                G_passwordChar = value ? '\0' : '*';
            }
        }
    }
}
