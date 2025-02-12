﻿namespace client.forms
{
    partial class RoomsForm
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
            roomsControl1 = new controls.RoomsControl();
            TopLayout = new FlowLayoutPanel();
            menuStrip1 = new MenuStrip();
            roomToolStripMenuItem = new ToolStripMenuItem();
            joinToolStripMenuItem = new ToolStripMenuItem();
            createToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
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
            MainPanel.Size = new Size(1066, 630);
            MainPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Silver;
            panel1.Controls.Add(roomsControl1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(1066, 603);
            panel1.TabIndex = 1;
            // 
            // roomsControl1
            // 
            roomsControl1.Dock = DockStyle.Fill;
            roomsControl1.Location = new Point(0, 0);
            roomsControl1.Name = "roomsControl1";
            roomsControl1.Size = new Size(1066, 603);
            roomsControl1.TabIndex = 0;
            // 
            // TopLayout
            // 
            TopLayout.BackColor = Color.Transparent;
            TopLayout.Controls.Add(menuStrip1);
            TopLayout.Dock = DockStyle.Top;
            TopLayout.Location = new Point(0, 0);
            TopLayout.Name = "TopLayout";
            TopLayout.Size = new Size(1066, 27);
            TopLayout.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.Fill;
            menuStrip1.GripStyle = ToolStripGripStyle.Visible;
            menuStrip1.Items.AddRange(new ToolStripItem[] { roomToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            menuStrip1.Size = new Size(244, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // roomToolStripMenuItem
            // 
            roomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { joinToolStripMenuItem, createToolStripMenuItem, toolStripSeparator1, refreshToolStripMenuItem });
            roomToolStripMenuItem.Name = "roomToolStripMenuItem";
            roomToolStripMenuItem.Size = new Size(51, 20);
            roomToolStripMenuItem.Text = "Room";
            // 
            // joinToolStripMenuItem
            // 
            joinToolStripMenuItem.Name = "joinToolStripMenuItem";
            joinToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.J;
            joinToolStripMenuItem.Size = new Size(180, 22);
            joinToolStripMenuItem.Text = "Join";
            joinToolStripMenuItem.Click += joinToolStripMenuItem_Click;
            // 
            // createToolStripMenuItem
            // 
            createToolStripMenuItem.Name = "createToolStripMenuItem";
            createToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Alt | Keys.C;
            createToolStripMenuItem.Size = new Size(180, 22);
            createToolStripMenuItem.Text = "Create";
            createToolStripMenuItem.Click += createToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            refreshToolStripMenuItem.Size = new Size(180, 22);
            refreshToolStripMenuItem.Text = "Restart";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1066, 630);
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
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Panel panel1;
        private controls.RoomsControl roomsControl1;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
    }
}
