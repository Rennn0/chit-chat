using client.src.forms;

namespace client
{
    partial class ChatForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            SendTextBox = new RichTextBox();
            ReceiveTextBox = new RichTextBox();
            button2 = new Button();
            RoomLabel = new Label();
            UserLabel = new Label();
            button3 = new Button();
            UsernameBox = new TextBox();
            EmailBox = new TextBox();
            label3 = new Label();
            label4 = new Label();
            RoomBox = new TextBox();
            ExistingRoomBox = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(32, 247);
            button1.Name = "button1";
            button1.Size = new Size(230, 50);
            button1.TabIndex = 0;
            button1.Text = "Send";
            button1.UseVisualStyleBackColor = true;
            button1.Click += SendClicked;
            // 
            // SendTextBox
            // 
            SendTextBox.Location = new Point(32, 339);
            SendTextBox.Name = "SendTextBox";
            SendTextBox.Size = new Size(295, 127);
            SendTextBox.TabIndex = 1;
            SendTextBox.Text = "";
            // 
            // ReceiveTextBox
            // 
            ReceiveTextBox.Location = new Point(355, 339);
            ReceiveTextBox.Name = "ReceiveTextBox";
            ReceiveTextBox.Size = new Size(309, 127);
            ReceiveTextBox.TabIndex = 2;
            ReceiveTextBox.Text = "";
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(32, 97);
            button2.Name = "button2";
            button2.Size = new Size(230, 50);
            button2.TabIndex = 3;
            button2.Text = "Create acc";
            button2.UseVisualStyleBackColor = true;
            button2.Click += CreateAccClicked;
            // 
            // RoomLabel
            // 
            RoomLabel.AutoSize = true;
            RoomLabel.Location = new Point(468, 22);
            RoomLabel.Name = "RoomLabel";
            RoomLabel.Size = new Size(45, 15);
            RoomLabel.TabIndex = 4;
            RoomLabel.Text = "room : ";
            // 
            // UserLabel
            // 
            UserLabel.AutoSize = true;
            UserLabel.Location = new Point(32, 174);
            UserLabel.Name = "UserLabel";
            UserLabel.Size = new Size(35, 15);
            UserLabel.TabIndex = 5;
            UserLabel.Text = "user :";
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.Location = new Point(468, 97);
            button3.Name = "button3";
            button3.Size = new Size(230, 50);
            button3.TabIndex = 6;
            button3.Text = "Create room";
            button3.UseVisualStyleBackColor = true;
            button3.Click += CreateRoomClicked;
            // 
            // UsernameBox
            // 
            UsernameBox.BorderStyle = BorderStyle.FixedSingle;
            UsernameBox.Location = new Point(32, 27);
            UsernameBox.Name = "UsernameBox";
            UsernameBox.Size = new Size(227, 23);
            UsernameBox.TabIndex = 7;
            // 
            // mainFormBindingsBindingSource
            // 
            // 
            // EmailBox
            // 
            EmailBox.Location = new Point(32, 68);
            EmailBox.Name = "EmailBox";
            EmailBox.Size = new Size(227, 23);
            EmailBox.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 50);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 9;
            label3.Text = "email ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 9);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 10;
            label4.Text = "username ";
            // 
            // RoomBox
            // 
            RoomBox.Location = new Point(468, 68);
            RoomBox.Name = "RoomBox";
            RoomBox.Size = new Size(227, 23);
            RoomBox.TabIndex = 11;
            // 
            // ExistingRoomBox
            // 
            ExistingRoomBox.Location = new Point(286, 274);
            ExistingRoomBox.Name = "ExistingRoomBox";
            ExistingRoomBox.PlaceholderText = "existing room id";
            ExistingRoomBox.Size = new Size(227, 23);
            ExistingRoomBox.TabIndex = 12;
            ExistingRoomBox.TextChanged += ExistingRoomBox_TextChanged;
            // 
            // mainFormBindingsBindingSource1
            // 
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(749, 666);
            Controls.Add(ExistingRoomBox);
            Controls.Add(RoomBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(EmailBox);
            Controls.Add(button3);
            Controls.Add(UserLabel);
            Controls.Add(RoomLabel);
            Controls.Add(button2);
            Controls.Add(ReceiveTextBox);
            Controls.Add(SendTextBox);
            Controls.Add(button1);
            Controls.Add(UsernameBox);
            Name = "ChatForm";
            Text = "ChitChat";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private RichTextBox SendTextBox;
        private RichTextBox ReceiveTextBox;
        private Button button2;
        private Label RoomLabel;
        private Label UserLabel;
        private Button button3;
        private TextBox UsernameBox;
        private TextBox EmailBox;
        private Label label3;
        private Label label4;
        private TextBox RoomBox;
        private TextBox ExistingRoomBox;
    }
}
