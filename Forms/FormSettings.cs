using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;

namespace MacroBot.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            checkBoxHideWhenMinimized.Checked = Program.Settings.HideWhenMinimized;

            textBoxRecordHotkey.Text = Program.Settings.RecordHotkey.ToString();
            textBoxPlayHotkey.Text = Program.Settings.PlayHotkey.ToString();
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
                textBoxRecordHotkey.InvokeSetText(e.KeyCode.ToString());
            }).Start();
        }
        private void textBoxPlayHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            new Thread(() =>
            {
                Thread.Sleep(15);
                textBoxPlayHotkey.InvokeSetText(e.KeyCode.ToString());
            }).Start();
        }
    }
}
