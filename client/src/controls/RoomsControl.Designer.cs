namespace client.controls
{
    partial class RoomsControl
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
            components = new System.ComponentModel.Container();
            bindings.RoomsControlBinding roomsControlBinding1 = new bindings.RoomsControlBinding();
            MainPanel = new Panel();
            RoomDataGrid = new DataGridView();
            groomIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            RoomMenuStrip = new ContextMenuStrip(components);
            joinToolStripMenuItem = new ToolStripMenuItem();
            groomNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            ghostUserIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            gdescriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            gparticipantsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            roomsControlBindingBindingSource = new BindingSource(components);
            MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RoomDataGrid).BeginInit();
            RoomMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roomsControlBindingBindingSource).BeginInit();
            SuspendLayout();
            // 
            // MainPanel
            // 
            MainPanel.Controls.Add(RoomDataGrid);
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(726, 490);
            MainPanel.TabIndex = 0;
            // 
            // RoomDataGrid
            // 
            RoomDataGrid.AllowUserToAddRows = false;
            RoomDataGrid.AllowUserToDeleteRows = false;
            RoomDataGrid.AutoGenerateColumns = false;
            RoomDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            RoomDataGrid.Columns.AddRange(new DataGridViewColumn[] { groomIdDataGridViewTextBoxColumn, groomNameDataGridViewTextBoxColumn, ghostUserIdDataGridViewTextBoxColumn, gdescriptionDataGridViewTextBoxColumn, gparticipantsDataGridViewTextBoxColumn });
            RoomDataGrid.DataSource = roomsControlBindingBindingSource;
            RoomDataGrid.Dock = DockStyle.Fill;
            RoomDataGrid.Location = new Point(0, 0);
            RoomDataGrid.MultiSelect = false;
            RoomDataGrid.Name = "RoomDataGrid";
            RoomDataGrid.Size = new Size(726, 490);
            RoomDataGrid.TabIndex = 0;
            // 
            // groomIdDataGridViewTextBoxColumn
            // 
            groomIdDataGridViewTextBoxColumn.ContextMenuStrip = RoomMenuStrip;
            groomIdDataGridViewTextBoxColumn.DataPropertyName = "G_roomId";
            groomIdDataGridViewTextBoxColumn.HeaderText = "Room";
            groomIdDataGridViewTextBoxColumn.Name = "groomIdDataGridViewTextBoxColumn";
            // 
            // RoomMenuStrip
            // 
            RoomMenuStrip.Items.AddRange(new ToolStripItem[] { joinToolStripMenuItem });
            RoomMenuStrip.Name = "RoomMenuStrip";
            RoomMenuStrip.Size = new Size(181, 48);
            // 
            // joinToolStripMenuItem
            // 
            joinToolStripMenuItem.Name = "joinToolStripMenuItem";
            joinToolStripMenuItem.Size = new Size(180, 22);
            joinToolStripMenuItem.Text = "Join";
            // 
            // groomNameDataGridViewTextBoxColumn
            // 
            groomNameDataGridViewTextBoxColumn.DataPropertyName = "G_roomName";
            groomNameDataGridViewTextBoxColumn.HeaderText = "Name";
            groomNameDataGridViewTextBoxColumn.Name = "groomNameDataGridViewTextBoxColumn";
            // 
            // ghostUserIdDataGridViewTextBoxColumn
            // 
            ghostUserIdDataGridViewTextBoxColumn.DataPropertyName = "G_hostUserId";
            ghostUserIdDataGridViewTextBoxColumn.HeaderText = "Host";
            ghostUserIdDataGridViewTextBoxColumn.Name = "ghostUserIdDataGridViewTextBoxColumn";
            // 
            // gdescriptionDataGridViewTextBoxColumn
            // 
            gdescriptionDataGridViewTextBoxColumn.DataPropertyName = "G_description";
            gdescriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            gdescriptionDataGridViewTextBoxColumn.Name = "gdescriptionDataGridViewTextBoxColumn";
            // 
            // gparticipantsDataGridViewTextBoxColumn
            // 
            gparticipantsDataGridViewTextBoxColumn.DataPropertyName = "G_participants";
            gparticipantsDataGridViewTextBoxColumn.HeaderText = "Active Members";
            gparticipantsDataGridViewTextBoxColumn.Name = "gparticipantsDataGridViewTextBoxColumn";
            // 
            // roomsControlBindingBindingSource
            // 
            roomsControlBinding1.G_description = "";
            roomsControlBinding1.G_hostUserId = "";
            roomsControlBinding1.G_participants = 0;
            roomsControlBinding1.G_roomId = "";
            roomsControlBinding1.G_roomName = "";
            roomsControlBindingBindingSource.DataSource = roomsControlBinding1;
            roomsControlBindingBindingSource.Position = 0;
            // 
            // RoomsControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MainPanel);
            Name = "RoomsControl";
            Size = new Size(726, 490);
            MainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)RoomDataGrid).EndInit();
            RoomMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)roomsControlBindingBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel MainPanel;
        private DataGridView RoomDataGrid;
        private BindingSource roomsControlBindingBindingSource;
        private ContextMenuStrip RoomMenuStrip;
        private ToolStripMenuItem joinToolStripMenuItem;
        private DataGridViewTextBoxColumn groomIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn groomNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn ghostUserIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn gdescriptionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn gparticipantsDataGridViewTextBoxColumn;
    }
}
