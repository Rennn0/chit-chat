namespace client.forms
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
            MainPanel = new Panel();
            panel1 = new Panel();
            TopLayout = new FlowLayoutPanel();
            menuStrip1 = new MenuStrip();
            roomToolStripMenuItem = new ToolStripMenuItem();
            joinToolStripMenuItem = new ToolStripMenuItem();
            createToolStripMenuItem = new ToolStripMenuItem();
            listOnlineToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            roomsControl1 = new controls.RoomsControl();
            MainPanel.SuspendLayout();
            panel1.SuspendLayout();
            TopLayout.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // MainPanel
            // 
            MainPanel.Controls.Add(panel1);
            MainPanel.Controls.Add(TopLayout);
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(762, 611);
            MainPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Silver;
            panel1.Controls.Add(roomsControl1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(762, 584);
            panel1.TabIndex = 1;
            // 
            // TopLayout
            // 
            TopLayout.BackColor = Color.Transparent;
            TopLayout.Controls.Add(menuStrip1);
            TopLayout.Dock = DockStyle.Top;
            TopLayout.Location = new Point(0, 0);
            TopLayout.Name = "TopLayout";
            TopLayout.Size = new Size(762, 27);
            TopLayout.TabIndex = 0;
            TopLayout.Paint += TopLayout_Paint;
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.Fill;
            menuStrip1.GripStyle = ToolStripGripStyle.Visible;
            menuStrip1.Items.AddRange(new ToolStripItem[] { roomToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            menuStrip1.Size = new Size(124, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // roomToolStripMenuItem
            // 
            roomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { joinToolStripMenuItem, createToolStripMenuItem, listOnlineToolStripMenuItem });
            roomToolStripMenuItem.Name = "roomToolStripMenuItem";
            roomToolStripMenuItem.Size = new Size(51, 20);
            roomToolStripMenuItem.Text = "Room";
            // 
            // joinToolStripMenuItem
            // 
            joinToolStripMenuItem.Name = "joinToolStripMenuItem";
            joinToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.J;
            joinToolStripMenuItem.Size = new Size(173, 22);
            joinToolStripMenuItem.Text = "Join";
            joinToolStripMenuItem.Click += joinToolStripMenuItem_Click;
            // 
            // createToolStripMenuItem
            // 
            createToolStripMenuItem.Name = "createToolStripMenuItem";
            createToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Alt | Keys.C;
            createToolStripMenuItem.Size = new Size(173, 22);
            createToolStripMenuItem.Text = "Create";
            createToolStripMenuItem.Click += createToolStripMenuItem_Click;
            // 
            // listOnlineToolStripMenuItem
            // 
            listOnlineToolStripMenuItem.Name = "listOnlineToolStripMenuItem";
            listOnlineToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.L;
            listOnlineToolStripMenuItem.Size = new Size(173, 22);
            listOnlineToolStripMenuItem.Text = "List online";
            listOnlineToolStripMenuItem.Click += listOnlineToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // roomsControl1
            // 
            roomsControl1.Dock = DockStyle.Fill;
            roomsControl1.Location = new Point(0, 0);
            roomsControl1.Name = "roomsControl1";
            roomsControl1.Size = new Size(762, 584);
            roomsControl1.TabIndex = 0;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(762, 611);
            Controls.Add(MainPanel);
            Name = "ChatForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChitChat";
            Load += MainForm_Load;
            MainPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            TopLayout.ResumeLayout(false);
            TopLayout.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel MainPanel;
        private FlowLayoutPanel TopLayout;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem roomToolStripMenuItem;
        private ToolStripMenuItem joinToolStripMenuItem;
        private ToolStripMenuItem createToolStripMenuItem;
        private ToolStripMenuItem listOnlineToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Panel panel1;
        private controls.RoomsControl roomsControl1;
    }
}
