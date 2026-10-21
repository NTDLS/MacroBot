namespace MacroBot.Forms
{
    partial class FormEditRecording
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditRecording));
            labelName = new Label();
            labelSpeed = new Label();
            textBoxName = new TextBox();
            textBoxSpeed = new TextBox();
            buttonSave = new Button();
            buttonCancel = new Button();
            textBoxRepetitions = new TextBox();
            labelRepetitions = new Label();
            textBoxRepetitionDelay = new TextBox();
            labelRepetitionDelay = new Label();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(9, 15);
            labelName.Name = "labelName";
            labelName.Size = new Size(39, 15);
            labelName.TabIndex = 0;
            labelName.Text = "Name";
            // 
            // labelSpeed
            // 
            labelSpeed.AutoSize = true;
            labelSpeed.Location = new Point(10, 61);
            labelSpeed.Name = "labelSpeed";
            labelSpeed.Size = new Size(39, 15);
            labelSpeed.TabIndex = 1;
            labelSpeed.Text = "Speed";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(12, 33);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(100, 23);
            textBoxName.TabIndex = 0;
            // 
            // textBoxSpeed
            // 
            textBoxSpeed.Location = new Point(12, 79);
            textBoxSpeed.Name = "textBoxSpeed";
            textBoxSpeed.Size = new Size(100, 23);
            textBoxSpeed.TabIndex = 2;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(82, 114);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 4;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(163, 114);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 5;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // textBoxRepetitions
            // 
            textBoxRepetitions.Location = new Point(138, 33);
            textBoxRepetitions.Name = "textBoxRepetitions";
            textBoxRepetitions.Size = new Size(100, 23);
            textBoxRepetitions.TabIndex = 1;
            // 
            // labelRepetitions
            // 
            labelRepetitions.AutoSize = true;
            labelRepetitions.Location = new Point(136, 15);
            labelRepetitions.Name = "labelRepetitions";
            labelRepetitions.Size = new Size(66, 15);
            labelRepetitions.TabIndex = 6;
            labelRepetitions.Text = "Repetitions";
            // 
            // textBoxRepetitionDelay
            // 
            textBoxRepetitionDelay.Location = new Point(138, 79);
            textBoxRepetitionDelay.Name = "textBoxRepetitionDelay";
            textBoxRepetitionDelay.Size = new Size(100, 23);
            textBoxRepetitionDelay.TabIndex = 3;
            // 
            // labelRepetitionDelay
            // 
            labelRepetitionDelay.AutoSize = true;
            labelRepetitionDelay.Location = new Point(136, 61);
            labelRepetitionDelay.Name = "labelRepetitionDelay";
            labelRepetitionDelay.Size = new Size(93, 15);
            labelRepetitionDelay.TabIndex = 8;
            labelRepetitionDelay.Text = "Repetition Delay";
            // 
            // FormEditRecording
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(253, 149);
            Controls.Add(textBoxRepetitionDelay);
            Controls.Add(labelRepetitionDelay);
            Controls.Add(textBoxRepetitions);
            Controls.Add(labelRepetitions);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            Controls.Add(textBoxSpeed);
            Controls.Add(textBoxName);
            Controls.Add(labelSpeed);
            Controls.Add(labelName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEditRecording";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Recording";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelName;
        private Label labelSpeed;
        private TextBox textBoxName;
        private TextBox textBoxSpeed;
        private Button buttonSave;
        private Button buttonCancel;
        private TextBox textBoxRepetitions;
        private Label labelRepetitions;
        private TextBox textBoxRepetitionDelay;
        private Label labelRepetitionDelay;
    }
}