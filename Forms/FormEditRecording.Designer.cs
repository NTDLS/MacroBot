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
            labelActions = new Label();
            textBoxActions = new TextBox();
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
            labelName.Location = new Point(11, 15);
            labelName.Name = "labelName";
            labelName.Size = new Size(39, 15);
            labelName.TabIndex = 0;
            labelName.Text = "Name";
            // 
            // labelSpeed
            // 
            labelSpeed.AutoSize = true;
            labelSpeed.Location = new Point(12, 54);
            labelSpeed.Name = "labelSpeed";
            labelSpeed.Size = new Size(39, 15);
            labelSpeed.TabIndex = 1;
            labelSpeed.Text = "Speed";
            // 
            // labelActions
            // 
            labelActions.AutoSize = true;
            labelActions.Location = new Point(12, 97);
            labelActions.Name = "labelActions";
            labelActions.Size = new Size(47, 15);
            labelActions.TabIndex = 2;
            labelActions.Text = "Actions";
            // 
            // textBoxActions
            // 
            textBoxActions.AcceptsTab = true;
            textBoxActions.Location = new Point(12, 115);
            textBoxActions.Multiline = true;
            textBoxActions.Name = "textBoxActions";
            textBoxActions.ScrollBars = ScrollBars.Both;
            textBoxActions.Size = new Size(357, 323);
            textBoxActions.TabIndex = 2;
            textBoxActions.WordWrap = false;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(55, 12);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(100, 23);
            textBoxName.TabIndex = 0;
            // 
            // textBoxSpeed
            // 
            textBoxSpeed.Location = new Point(55, 51);
            textBoxSpeed.Name = "textBoxSpeed";
            textBoxSpeed.Size = new Size(100, 23);
            textBoxSpeed.TabIndex = 1;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(213, 450);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 3;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(294, 450);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // textBoxRepetitions
            // 
            textBoxRepetitions.Location = new Point(269, 12);
            textBoxRepetitions.Name = "textBoxRepetitions";
            textBoxRepetitions.Size = new Size(100, 23);
            textBoxRepetitions.TabIndex = 5;
            // 
            // labelRepetitions
            // 
            labelRepetitions.AutoSize = true;
            labelRepetitions.Location = new Point(194, 15);
            labelRepetitions.Name = "labelRepetitions";
            labelRepetitions.Size = new Size(66, 15);
            labelRepetitions.TabIndex = 6;
            labelRepetitions.Text = "Repetitions";
            // 
            // textBoxRepetitionDelay
            // 
            textBoxRepetitionDelay.Location = new Point(269, 51);
            textBoxRepetitionDelay.Name = "textBoxRepetitionDelay";
            textBoxRepetitionDelay.Size = new Size(100, 23);
            textBoxRepetitionDelay.TabIndex = 7;
            // 
            // labelRepetitionDelay
            // 
            labelRepetitionDelay.AutoSize = true;
            labelRepetitionDelay.Location = new Point(170, 54);
            labelRepetitionDelay.Name = "labelRepetitionDelay";
            labelRepetitionDelay.Size = new Size(93, 15);
            labelRepetitionDelay.TabIndex = 8;
            labelRepetitionDelay.Text = "Repetition Delay";
            // 
            // FormEditRecording
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(388, 485);
            Controls.Add(textBoxRepetitionDelay);
            Controls.Add(labelRepetitionDelay);
            Controls.Add(textBoxRepetitions);
            Controls.Add(labelRepetitions);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            Controls.Add(textBoxSpeed);
            Controls.Add(textBoxName);
            Controls.Add(textBoxActions);
            Controls.Add(labelActions);
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
        private Label labelActions;
        private TextBox textBoxActions;
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