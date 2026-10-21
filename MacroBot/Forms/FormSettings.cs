using NTDLS.Localization;
using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;

namespace MacroBot.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            FormLocalizer.Apply(this);
            Theming.InitializeApplication();

            AcceptButton = buttonSave;
            CancelButton = buttonCancel;

            checkBoxHideWhenMinimized.Checked = Program.Settings.HideWhenMinimized;
            checkBoxRelativeMouse.Checked = Program.Settings.RelativeMouseMode;

            textBoxRecordHotkey.Text = Program.Settings.RecordHotkey.ToString();
            textBoxPlayHotkey.Text = Program.Settings.PlayHotkey.ToString();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Program.Settings.HideWhenMinimized = checkBoxHideWhenMinimized.Checked;
            Program.Settings.RelativeMouseMode = checkBoxRelativeMouse.Checked;

            try
            {
                Program.Settings.RecordHotkey = Enum.Parse<Keys>(textBoxRecordHotkey.Text).ToString();
            }
            catch
            {
                MessageBox.Show(Localize("The [record] hot key you have specified is not valid."), "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Program.Settings.PlayHotkey = Enum.Parse<Keys>(textBoxPlayHotkey.Text).ToString();
            }
            catch
            {
                MessageBox.Show(Localize("The [play] hot key you have specified is not valid."), "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LocalUserApplicationData.SaveToDisk("MacroBot", Program.Settings);

            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TextBoxRecordHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            new Thread(() =>
            {
                Thread.Sleep(15);
                textBoxRecordHotkey.InvokeSetText(e.KeyCode.ToString());
            }).Start();
        }
        private void TextBoxPlayHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            new Thread(() =>
            {
                Thread.Sleep(15);
                textBoxPlayHotkey.InvokeSetText(e.KeyCode.ToString());
            }).Start();
        }
    }
}
