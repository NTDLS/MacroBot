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
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            groupBoxControls.SuspendLayout();
            groupBoxHistory.SuspendLayout();
            SuspendLayout();
            // 
            // buttonRecord
            // 
            buttonRecord.Location = new Point(6, 32);
            buttonRecord.Name = "buttonRecord";
            buttonRecord.Size = new Size(77, 27);
            buttonRecord.TabIndex = 0;
            buttonRecord.Text = "Record";
            buttonRecord.UseVisualStyleBackColor = true;
            buttonRecord.Click += buttonRecord_Click;
            // 
            // buttonStopRecord
            // 
            buttonStopRecord.Location = new Point(89, 32);
            buttonStopRecord.Name = "buttonStopRecord";
            buttonStopRecord.Size = new Size(77, 27);
            buttonStopRecord.TabIndex = 1;
            buttonStopRecord.Text = "Stop";
            buttonStopRecord.UseVisualStyleBackColor = true;
            buttonStopRecord.Click += buttonStopRecord_Click;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(6, 78);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(77, 27);
            buttonPlay.TabIndex = 2;
            buttonPlay.Text = "Play";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += buttonPlay_Click;
            // 
            // buttonStopPlay
            // 
            buttonStopPlay.Location = new Point(89, 78);
            buttonStopPlay.Name = "buttonStopPlay";
            buttonStopPlay.Size = new Size(77, 27);
            buttonStopPlay.TabIndex = 3;
            buttonStopPlay.Text = "Stop";
            buttonStopPlay.UseVisualStyleBackColor = true;
            buttonStopPlay.Click += buttonStopPlay_Click;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = Properties.Resources.Full;
            pictureBoxLogo.Location = new Point(12, 22);
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
            groupBoxControls.Location = new Point(12, 190);
            groupBoxControls.Name = "groupBoxControls";
            groupBoxControls.Size = new Size(177, 129);
            groupBoxControls.TabIndex = 6;
            groupBoxControls.TabStop = false;
            groupBoxControls.Text = "Controls";
            // 
            // groupBoxHistory
            // 
            groupBoxHistory.Controls.Add(listViewHistory);
            groupBoxHistory.Location = new Point(195, 12);
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
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(587, 340);
            Controls.Add(groupBoxHistory);
            Controls.Add(groupBoxControls);
            Controls.Add(pictureBoxLogo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MacroBot";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            groupBoxControls.ResumeLayout(false);
            groupBoxHistory.ResumeLayout(false);
            ResumeLayout(false);
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
    }
}
