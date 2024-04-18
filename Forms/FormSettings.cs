using NTDLS.Persistence;

namespace MacroBot.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            checkBoxHideWhenMinimized.Checked = Program.Settings.HideWhenMinimized;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Program.Settings.HideWhenMinimized = checkBoxHideWhenMinimized.Checked;

            try
            {
                Program.Settings.RecordHotkey = Enum.Parse<Keys>(textBoxRecordHotkey.Text).ToString();
            }
            catch
            {
                MessageBox.Show("The [record] hot key you have specified is not valid.", "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Program.Settings.PlayHotkey = Enum.Parse<Keys>(textBoxPlayHotkey.Text).ToString();
            }
            catch
            {
                MessageBox.Show("The [play] hot key you have specified is not valid.", "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LocalUserApplicationData.SaveToDisk("MacroBot", Program.Settings);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxRecordHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            new Thread(() =>
            {
                Thread.Sleep(15);
                SetText(textBoxRecordHotkey, e.KeyCode.ToString());
            }).Start();
        }

        private void SetText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(() => SetText(control, text));
                return;
            }
            control.Text = text;
        }

        private void textBoxPlayHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            new Thread(() =>
            {
                Thread.Sleep(15);
                SetText(textBoxRecordHotkey, e.KeyCode.ToString());
            }).Start();
        }
    }
}
