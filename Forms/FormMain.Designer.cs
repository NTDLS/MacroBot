namespace MacroBot
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            buttonRecord = new Button();
            buttonStopRecord = new Button();
            buttonPlay = new Button();
            buttonStopPlay = new Button();
            pictureBoxLogo = new PictureBox();
            groupBoxControls = new GroupBox();
            groupBoxHistory = new GroupBox();
            listViewHistory = new ListView();
            columnHeaderName = new ColumnHeader();
            columnHeaderDate = new ColumnHeader();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            notifyIconMain = new NotifyIcon(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            groupBoxControls.SuspendLayout();
            groupBoxHistory.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonRecord
            // 
            buttonRecord.Location = new Point(6, 32);
            buttonRecord.Name = "buttonRecord";
            buttonRecord.Size = new Size(79, 27);
            buttonRecord.TabIndex = 0;
            buttonRecord.Text = "Record";
            buttonRecord.UseVisualStyleBackColor = true;
            buttonRecord.Click += ButtonRecord_Click;
            // 
            // buttonStopRecord
            // 
            buttonStopRecord.Location = new Point(90, 32);
            buttonStopRecord.Name = "buttonStopRecord";
            buttonStopRecord.Size = new Size(79, 27);
            buttonStopRecord.TabIndex = 1;
            buttonStopRecord.Text = "Stop";
            buttonStopRecord.UseVisualStyleBackColor = true;
            buttonStopRecord.Click += ButtonStopRecord_Click;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(6, 78);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(79, 27);
            buttonPlay.TabIndex = 2;
            buttonPlay.Text = "Play";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += ButtonPlay_Click;
            // 
            // buttonStopPlay
            // 
            buttonStopPlay.Location = new Point(90, 78);
            buttonStopPlay.Name = "buttonStopPlay";
            buttonStopPlay.Size = new Size(79, 27);
            buttonStopPlay.TabIndex = 3;
            buttonStopPlay.Text = "Stop";
            buttonStopPlay.UseVisualStyleBackColor = true;
            buttonStopPlay.Click += ButtonStopPlay_Click;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = Properties.Resources.Full;
            pictureBoxLogo.Location = new Point(12, 33);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(177, 162);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 4;
            pictureBoxLogo.TabStop = false;
            // 
            // groupBoxControls
            // 
            groupBoxControls.Controls.Add(buttonPlay);
            groupBoxControls.Controls.Add(buttonRecord);
            groupBoxControls.Controls.Add(buttonStopRecord);
            groupBoxControls.Controls.Add(buttonStopPlay);
            groupBoxControls.Location = new Point(12, 201);
            groupBoxControls.Name = "groupBoxControls";
            groupBoxControls.Size = new Size(177, 129);
            groupBoxControls.TabIndex = 6;
            groupBoxControls.TabStop = false;
            groupBoxControls.Text = "Controls";
            // 
            // groupBoxHistory
            // 
            groupBoxHistory.Controls.Add(listViewHistory);
            groupBoxHistory.Location = new Point(195, 23);
            groupBoxHistory.Name = "groupBoxHistory";
            groupBoxHistory.Size = new Size(383, 307);
            groupBoxHistory.TabIndex = 7;
            groupBoxHistory.TabStop = false;
            groupBoxHistory.Text = "History";
            // 
            // listViewHistory
            // 
            listViewHistory.CheckBoxes = true;
            listViewHistory.Columns.AddRange(new ColumnHeader[] { columnHeaderName, columnHeaderDate });
            listViewHistory.FullRowSelect = true;
            listViewHistory.GridLines = true;
            listViewHistory.LabelEdit = true;
            listViewHistory.Location = new Point(6, 22);
            listViewHistory.MultiSelect = false;
            listViewHistory.Name = "listViewHistory";
            listViewHistory.Size = new Size(371, 279);
            listViewHistory.TabIndex = 0;
            listViewHistory.UseCompatibleStateImageBehavior = false;
            listViewHistory.View = View.Details;
            // 
            // columnHeaderName
            // 
            columnHeaderName.Text = "Name";
            columnHeaderName.Width = 200;
            // 
            // columnHeaderDate
            // 
            columnHeaderDate.Text = "Date";
            columnHeaderDate.Width = 140;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(587, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // notifyIconMain
            // 
            notifyIconMain.BalloonTipText = "MacroBot";
            notifyIconMain.BalloonTipTitle = "MacroBot";
            notifyIconMain.Icon = (Icon)resources.GetObject("notifyIconMain.Icon");
            notifyIconMain.Text = "MacroBot";
            notifyIconMain.Visible = true;
            notifyIconMain.MouseClick += notifyIconMain_MouseClick;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(587, 347);
            Controls.Add(groupBoxHistory);
            Controls.Add(groupBoxControls);
            Controls.Add(pictureBoxLogo);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MacroBot";
            Load += FormMain_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            groupBoxControls.ResumeLayout(false);
            groupBoxHistory.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonRecord;
        private Button buttonStopRecord;
        private Button buttonPlay;
        private Button buttonStopPlay;
        private PictureBox pictureBoxLogo;
        private GroupBox groupBoxControls;
        private GroupBox groupBoxHistory;
        private ListView listViewHistory;
        private ColumnHeader columnHeaderName;
        private ColumnHeader columnHeaderDate;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private NotifyIcon notifyIconMain;
        private ToolStripMenuItem settingsToolStripMenuItem;
    }
}
