using MacroBot.Recording;
using Newtonsoft.Json;


namespace MacroBot.Forms
{
    internal partial class FormEditRecording : Form
    {
        public PersistedRecording? Recording { get; private set; }

        public FormEditRecording()
        {
            InitializeComponent();
        }

        public FormEditRecording(PersistedRecording recording)
        {
            InitializeComponent();

            Recording = recording;

            AcceptButton = buttonSave;
            CancelButton = buttonCancel;

            textBoxName.Text = recording.Name;
            textBoxSpeed.Text = recording.Speed.ToString();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            textBoxActions.Text = JsonConvert.SerializeObject(recording.Actions, settings);
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
                List<RepeatableAction>? actions;

                try
                {
                    actions = JsonConvert.DeserializeObject<List<RepeatableAction>>(textBoxActions.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"The recording text is invalid: {ex.Message}", "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxSpeed.Text, out var speed))
                {
                    speed = 1;
                }

                var newRecording = new PersistedRecording()
                {
                    CreatedDate = Recording.CreatedDate,
                    Name = textBoxName.Text,
                    Selected = Recording.Selected,
                    Speed = speed,
                    Actions = actions ?? new List<RepeatableAction>()
                };

                Recording = newRecording;

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
