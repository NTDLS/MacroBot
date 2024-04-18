namespace MacroBot.Forms
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            buttonSave = new Button();
            checkBoxHideWhenMinimized = new CheckBox();
            labelPlayHotkey = new Label();
            labelRecordHotkey = new Label();
            textBoxRecordHotkey = new TextBox();
            textBoxPlayHotkey = new TextBox();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(265, 146);
            buttonSave.Margin = new Padding(4, 3, 4, 3);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(88, 27);
            buttonSave.TabIndex = 7;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // checkBoxHideWhenMinimized
            // 
            checkBoxHideWhenMinimized.AutoSize = true;
            checkBoxHideWhenMinimized.Location = new Point(106, 97);
            checkBoxHideWhenMinimized.Name = "checkBoxHideWhenMinimized";
            checkBoxHideWhenMinimized.Size = new Size(198, 19);
            checkBoxHideWhenMinimized.TabIndex = 8;
            checkBoxHideWhenMinimized.Text = "Hide when recording or playing?";
            checkBoxHideWhenMinimized.UseVisualStyleBackColor = true;
            // 
            // labelPlayHotkey
            // 
            labelPlayHotkey.AutoSize = true;
            labelPlayHotkey.Location = new Point(27, 55);
            labelPlayHotkey.Name = "labelPlayHotkey";
            labelPlayHotkey.Size = new Size(73, 15);
            labelPlayHotkey.TabIndex = 9;
            labelPlayHotkey.Text = "Play Hotkey:";
            // 
            // labelRecordHotkey
            // 
            labelRecordHotkey.AutoSize = true;
            labelRecordHotkey.Location = new Point(12, 26);
            labelRecordHotkey.Name = "labelRecordHotkey";
            labelRecordHotkey.Size = new Size(88, 15);
            labelRecordHotkey.TabIndex = 10;
            labelRecordHotkey.Text = "Record Hotkey:";
            // 
            // textBoxRecordHotkey
            // 
            textBoxRecordHotkey.Location = new Point(106, 23);
            textBoxRecordHotkey.Name = "textBoxRecordHotkey";
            textBoxRecordHotkey.Size = new Size(100, 23);
            textBoxRecordHotkey.TabIndex = 11;
            textBoxRecordHotkey.PreviewKeyDown += textBoxRecordHotkey_PreviewKeyDown;
            // 
            // textBoxPlayHotkey
            // 
            textBoxPlayHotkey.Location = new Point(106, 52);
            textBoxPlayHotkey.Name = "textBoxPlayHotkey";
            textBoxPlayHotkey.Size = new Size(100, 23);
            textBoxPlayHotkey.TabIndex = 12;
            textBoxPlayHotkey.PreviewKeyDown += textBoxPlayHotkey_PreviewKeyDown;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(169, 146);
            buttonCancel.Margin = new Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(88, 27);
            buttonCancel.TabIndex = 13;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(366, 185);
            Controls.Add(buttonCancel);
            Controls.Add(textBoxPlayHotkey);
            Controls.Add(textBoxRecordHotkey);
            Controls.Add(labelRecordHotkey);
            Controls.Add(labelPlayHotkey);
            Controls.Add(checkBoxHideWhenMinimized);
            Controls.Add(buttonSave);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private Button buttonSave;
        private CheckBox checkBoxHideWhenMinimized;
        private Label labelPlayHotkey;
        private Label labelRecordHotkey;
        private TextBox textBoxRecordHotkey;
        private TextBox textBoxPlayHotkey;
        private Button buttonCancel;
    }
}