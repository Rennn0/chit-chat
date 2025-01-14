namespace client.controls
{
    partial class ChatControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MessagePanel = new FlowLayoutPanel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            attachToolStripMenuItem = new ToolStripMenuItem();
            listToolStripMenuItem = new ToolStripMenuItem();
            MessageTextBox = new TextBox();
            SendButton = new Button();
            MessagePanel.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // MessagePanel
            // 
            MessagePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MessagePanel.Controls.Add(menuStrip1);
            MessagePanel.FlowDirection = FlowDirection.TopDown;
            MessagePanel.Location = new Point(0, 0);
            MessagePanel.Name = "MessagePanel";
            MessagePanel.Size = new Size(723, 420);
            MessagePanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(165, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { attachToolStripMenuItem, listToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // attachToolStripMenuItem
            // 
            attachToolStripMenuItem.Name = "attachToolStripMenuItem";
            attachToolStripMenuItem.Size = new Size(180, 22);
            attachToolStripMenuItem.Text = "Attach";
            attachToolStripMenuItem.Click += attachToolStripMenuItem_Click;
            // 
            // listToolStripMenuItem
            // 
            listToolStripMenuItem.Name = "listToolStripMenuItem";
            listToolStripMenuItem.Size = new Size(180, 22);
            listToolStripMenuItem.Text = "List";
            // 
            // MessageTextBox
            // 
            MessageTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MessageTextBox.Font = new Font("Segoe UI", 12F);
            MessageTextBox.Location = new Point(3, 426);
            MessageTextBox.Multiline = true;
            MessageTextBox.Name = "MessageTextBox";
            MessageTextBox.Size = new Size(594, 44);
            MessageTextBox.TabIndex = 0;
            // 
            // SendButton
            // 
            SendButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SendButton.FlatAppearance.BorderColor = Color.Red;
            SendButton.FlatAppearance.BorderSize = 3;
            SendButton.Font = new Font("Segoe UI", 13F);
            SendButton.Location = new Point(603, 426);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(114, 43);
            SendButton.TabIndex = 1;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // ChatControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(SendButton);
            Controls.Add(MessageTextBox);
            Controls.Add(MessagePanel);
            Name = "ChatControl";
            Size = new Size(723, 473);
            MessagePanel.ResumeLayout(false);
            MessagePanel.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel MessagePanel;
        private TextBox MessageTextBox;
        private Button SendButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem attachToolStripMenuItem;
        private ToolStripMenuItem listToolStripMenuItem;
    }
}
