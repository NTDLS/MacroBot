using MacroBot.Recording;
using NTDLS.WinFormsHelpers;

namespace MacroBot.Forms
{
    internal partial class FormEditRecording : Form
    {
        public PersistedRecording? Recording { get; private set; }

        public FormEditRecording()
        {
            InitializeComponent();
            Theming.InitializeApplication();
        }

        public FormEditRecording(PersistedRecording recording)
        {
            InitializeComponent();
            Theming.InitializeApplication();

            Recording = recording;

            AcceptButton = buttonSave;
            CancelButton = buttonCancel;

            textBoxName.Text = recording.Name;
            textBoxSpeed.Text = recording.Speed.ToString();
            textBoxRepetitions.Text = recording.Repetitions.ToString();
            textBoxRepetitionDelay.Text = recording.RepetitionDelay.ToString();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (Recording != null)
            {
                if (!double.TryParse(textBoxSpeed.Text, out var speed))
                {
                    speed = 1;
                }

                if (!int.TryParse(textBoxRepetitions.Text, out var repetitions))
                {
                    repetitions = 0;
                }

                if (!int.TryParse(textBoxRepetitionDelay.Text, out var repetitionDelay))
                {
                    repetitionDelay = 1000;
                }

                Recording = new PersistedRecording()
                {
                    CreatedDate = Recording.CreatedDate,
                    Name = textBoxName.Text,
                    Selected = Recording.Selected,
                    MouseMode = Recording.MouseMode,
                    Speed = speed,
                    Repetitions = repetitions,
                    RepetitionDelay = repetitionDelay,
                    Actions = Recording.Actions
                };

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
