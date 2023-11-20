using NTDLS.Persistence;
using System.Collections.Generic;

namespace MacroBot
{
    public partial class FormMain : Form
    {
        private readonly ActionRecorder _actionRecorder = new();
        private readonly ActionPlayer _actionPlayer = new();

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyboardHook.OnKeyboardEventInterceptor += KeyboardHook_OnKeyboardEventInterceptor;
            listViewHistory.AfterLabelEdit += (object? sender, LabelEditEventArgs e) =>
            {
                listViewHistory.Items[e.Item].Text = e.Label;

                SaveRecordings();
            };

            LoadRecordings();
            ToggleFormVisualStates();
            _actionPlayer.OnStopped += _actionPlayer_OnStopped;
        }


        private void _actionPlayer_OnStopped(ActionPlayer sender)
        {
            if (InvokeRequired)
            {
                Invoke(() => _actionPlayer_OnStopped(sender));
                return;
            }

            ToggleFormVisualStates();
        }

        private void KeyboardHook_OnKeyboardEventInterceptor(Keys key, Win32s.ButtonDisposition keyboardButtonDirection)
        {
            if (key == Keys.F6 && keyboardButtonDirection == Win32s.ButtonDisposition.Up)
            {
                if (_actionPlayer.IsRunning)
                {
                    //We cannot start/stop the recorder while the player is running.
                    return;
                }

                if (_actionRecorder.IsRunning)
                    StopRecord();
                else StartRecord();

            }
            else if (key == Keys.F7 && keyboardButtonDirection == Win32s.ButtonDisposition.Up)
            {
                if (_actionRecorder.IsRunning)
                {
                    //We cannot start/stop the player while the recorder is running.
                    return;
                }

                if (_actionPlayer.IsRunning)
                    StopPlay();
                else StartPlay();
            }
        }

        private void ToggleFormVisualStates()
        {
            buttonRecord.BackColor = _actionRecorder.IsRunning ? Color.LightGreen : SystemColors.Control;
            buttonStopRecord.BackColor = _actionRecorder.IsRunning ? Color.IndianRed : SystemColors.Control;

            buttonPlay.BackColor = _actionPlayer.IsRunning ? Color.LightGreen : SystemColors.Control;
            buttonStopPlay.BackColor = _actionPlayer.IsRunning ? Color.IndianRed : SystemColors.Control;

            buttonRecord.Enabled = !_actionRecorder.IsRunning && !_actionPlayer.IsRunning;
            buttonStopRecord.Enabled = _actionRecorder.IsRunning;

            buttonPlay.Enabled = !_actionRecorder.IsRunning && !_actionPlayer.IsRunning;
            buttonStopPlay.Enabled = _actionPlayer.IsRunning;
        }

        private void StartRecord()
        {
            _actionRecorder.Start();
            ToggleFormVisualStates();
        }

        private void StopRecord()
        {
            _actionRecorder.Stop();

            var recording = new PersistedRecording()
            {
                Name = $"Recording {(listViewHistory.Items.Count + 1):n0}",
                CreatedDate = DateTime.Now,
                Actions = _actionRecorder.Actions
            };

            var item = new ListViewItem(new string[] { recording.Name, recording.SafeDateTimeName() });
            listViewHistory.Items.Add(item).Tag = recording;

            SaveRecordings();

            ToggleFormVisualStates();
        }

        private void SaveRecordings()
        {
            List<PersistedRecording> recordings = new();

            foreach (ListViewItem item in listViewHistory.Items)
            {
                var rowPersist = (PersistedRecording?)item.Tag;
                if (rowPersist != null)
                {
                    rowPersist.Name = item.Text;
                    recordings.Add(rowPersist);
                }
            }

            ApplicationData.SaveToDisk("MacroBot", recordings);
        }

        private void LoadRecordings()
        {
            var recordings = ApplicationData.LoadFromDisk("MacroBot", new List<PersistedRecording>());

            foreach (var recording in recordings)
            {
                var item = new ListViewItem(new string[] { recording.Name, recording.SafeDateTimeName() });
                listViewHistory.Items.Add(item).Tag = recording;
            }
        }

        private void StartPlay()
        {
            _actionPlayer.Start();
            ToggleFormVisualStates();
        }

        private void StopPlay()
        {
            _actionPlayer.Stop();
            ToggleFormVisualStates();
        }

        private void buttonRecord_Click(object sender, EventArgs e) => StartRecord();
        private void buttonStopRecord_Click(object sender, EventArgs e) => StopRecord();
        private void buttonPlay_Click(object sender, EventArgs e) => StartPlay();
        private void buttonStopPlay_Click(object sender, EventArgs e) => StopPlay();
    }
}
