namespace client.forms
{
    partial class AuthorizationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            bindings.AuthorizationBinding authorizationBinding1 = new bindings.AuthorizationBinding();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorizationForm));
            UsernameBox = new TextBox();
            authorizationBindingBindingSource = new BindingSource(components);
            PasswordBox = new TextBox();
            AutoFillButton = new Button();
            PasswordCheckBox = new CheckBox();
            EnterButton = new Button();
            ((System.ComponentModel.ISupportInitialize)authorizationBindingBindingSource).BeginInit();
            SuspendLayout();
            // 
            // UsernameBox
            // 
            UsernameBox.DataBindings.Add(new Binding("Text", authorizationBindingBindingSource, "G_username", true, DataSourceUpdateMode.OnPropertyChanged));
            UsernameBox.Location = new Point(25, 25);
            UsernameBox.Name = "UsernameBox";
            UsernameBox.PlaceholderText = "Username ";
            UsernameBox.Size = new Size(240, 23);
            UsernameBox.TabIndex = 1;
            // 
            // authorizationBindingBindingSource
            // 
            authorizationBinding1.G_password = "";
            authorizationBinding1.G_passwordChar = '*';
            authorizationBinding1.G_userId = "";
            authorizationBinding1.G_username = "";
            authorizationBinding1.PasswordChecked = false;
            authorizationBindingBindingSource.DataSource = authorizationBinding1;
            authorizationBindingBindingSource.Position = 0;
            // 
            // PasswordBox
            // 
            PasswordBox.DataBindings.Add(new Binding("Text", authorizationBindingBindingSource, "G_password", true, DataSourceUpdateMode.OnPropertyChanged));
            PasswordBox.DataBindings.Add(new Binding("PasswordChar", authorizationBindingBindingSource, "G_passwordChar", true, DataSourceUpdateMode.OnPropertyChanged));
            PasswordBox.Location = new Point(25, 71);
            PasswordBox.Name = "PasswordBox";
            PasswordBox.PasswordChar = '#';
            PasswordBox.PlaceholderText = "Password";
            PasswordBox.Size = new Size(240, 23);
            PasswordBox.TabIndex = 2;
            // 
            // AutoFillButton
            // 
            AutoFillButton.Location = new Point(315, 25);
            AutoFillButton.Name = "AutoFillButton";
            AutoFillButton.Size = new Size(148, 23);
            AutoFillButton.TabIndex = 4;
            AutoFillButton.Text = "Randomize";
            AutoFillButton.UseVisualStyleBackColor = true;
            AutoFillButton.Click += AutoFillButton_Click;
            // 
            // PasswordCheckBox
            // 
            PasswordCheckBox.AutoSize = true;
            PasswordCheckBox.DataBindings.Add(new Binding("Checked", authorizationBindingBindingSource, "PasswordChecked", true, DataSourceUpdateMode.OnPropertyChanged));
            PasswordCheckBox.Location = new Point(271, 75);
            PasswordCheckBox.Name = "PasswordCheckBox";
            PasswordCheckBox.Size = new Size(15, 14);
            PasswordCheckBox.TabIndex = 3;
            PasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // EnterButton
            // 
            EnterButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            EnterButton.Location = new Point(149, 211);
            EnterButton.Name = "EnterButton";
            EnterButton.Size = new Size(167, 29);
            EnterButton.TabIndex = 5;
            EnterButton.Text = "Enter";
            EnterButton.UseVisualStyleBackColor = true;
            EnterButton.Click += EnterButton_Click;
            // 
            // AuthorizationForm
            // 
            AcceptButton = EnterButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Snow;
            ClientSize = new Size(484, 261);
            ControlBox = false;
            Controls.Add(EnterButton);
            Controls.Add(PasswordCheckBox);
            Controls.Add(AutoFillButton);
            Controls.Add(PasswordBox);
            Controls.Add(UsernameBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            ImeMode = ImeMode.Hiragana;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AuthorizationForm";
            Text = "Gate";
            Load += AuthorizationForm_Load;
            ((System.ComponentModel.ISupportInitialize)authorizationBindingBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox UsernameBox;
        private TextBox PasswordBox;
        private Button AutoFillButton;
        private CheckBox PasswordCheckBox;
        private BindingSource authorizationBindingBindingSource;
        private Button EnterButton;
    }
}