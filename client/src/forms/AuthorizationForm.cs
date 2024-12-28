﻿using System.Media;
using client.bindings;
using client.Properties;
using Grpc.Net.Client;
using gRpcProtos;
using LLibrary.Guards;
using Timer = System.Threading.Timer;

namespace client.forms
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
            this.Icon = Resources.ButterflyIcon;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void AuthorizationForm_Load(object sender, EventArgs e) { }

        private void AutoFillButton_Click(object sender, EventArgs e)
        {
            AuthorizationBinding bind = Guard.AgainstNull(
                authorizationBindingBindingSource.Current as AuthorizationBinding
            );
            string[] randomUsernames = Resources.RandomUsernames.Split(',');
            Random r = new Random();
            byte[] b = new byte[16];
            r.NextBytes(b);

            bind.G_username =
                randomUsernames[r.Next(0, randomUsernames.Length)].Replace("\"", string.Empty)
                + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            bind.G_password = Convert.ToHexString(b);
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            GrpcChannel channel = GrpcChannel.ForAddress(
                Guard.AgainstNull(Properties.Resources.MessageServerUrl)
            );

            MessageExchangeService.MessageExchangeServiceClient client =
                new MessageExchangeService.MessageExchangeServiceClient(channel);

            AuthorizationBinding bind = Guard.AgainstNull(
                authorizationBindingBindingSource.Current as AuthorizationBinding
            );

            CreateUserResponse response = Guard.AgainstNull(
                client.CreateUser(
                    new CreateUserRequest { Password = bind.G_password, Username = bind.G_username }
                )
            );

            if (response.Code == CreateUserResponse.Types.CODE.UsernameUsed)
            {
                AutoFillButton_Click(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show(response.UserId);
            }

            //Settings.Default.token = response.UserId;
            //Settings.Default.Save();
        }
    }
}
