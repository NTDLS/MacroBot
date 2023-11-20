using MacroBot.Forms;
using MacroBot.Hooks;
using MacroBot.Recording;
using MacroBot.Win32;
using Newtonsoft.Json;
using NTDLS.Persistence;

namespace MacroBot
{
    public partial class FormMain : Form
    {
        private readonly ActionRecorder _actionRecorder = new();
        private readonly ActionPlayer _actionPlayer = new();
        private bool _isGridPopulating = false;

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

            listViewHistory.ItemChecked += ListViewHistory_ItemChecked;
            listViewHistory.MouseDown += ListViewHistory_MouseDown;
            listViewHistory.KeyUp += ListViewHistory_KeyUp;

            LoadRecordings();
            ToggleFormVisualStates();
            _actionPlayer.OnStopped += ActionPlayer_OnStopped;
        }

        private void ListViewHistory_MouseDown(object? sender, MouseEventArgs e)
        {
            _isGridPopulating = true;
            if (e.Button == MouseButtons.Left && e.Clicks == 2 && listViewHistory.SelectedItems.Count == 1)
            {
                var clickedItem = listViewHistory.SelectedItems[0];
                var recording = (PersistedRecording?)clickedItem.Tag;
                if (recording != null)
                {
                    using var form = new FormEditRecording(recording);
                    if (form.ShowDialog() == DialogResult.OK && form.Recording != null)
                    {
                        clickedItem.Text = form.Recording.Name;
                        clickedItem.Tag = form.Recording;
                        SaveRecordings();
                    }
                }
            }
            _isGridPopulating = false;
        }

        private void ListViewHistory_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (listViewHistory.SelectedItems.Count > 0)
                {
                    var item = listViewHistory.SelectedItems[0];
                    item.BeginEdit();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (listViewHistory.SelectedItems.Count > 0)
                {
                    var item = listViewHistory.SelectedItems[0];

                    if (MessageBox.Show($"Delete the recording '{item.Text}'", "MacroBot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                    listViewHistory.Items.Remove(item);

                    SaveRecordings();
                }
            }
        }

        private void ListViewHistory_ItemChecked(object? sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked == false || _isGridPopulating)
            {
                return;
            }

            foreach (ListViewItem item in listViewHistory.Items)
            {
                if (item.Index != e.Item.Index)
                {
                    item.Checked = false;
                }
            }
        }

        private void ActionPlayer_OnStopped(ActionPlayer sender)
        {
            if (InvokeRequired)
            {
                Invoke(() => ActionPlayer_OnStopped(sender));
                return;
            }

            ToggleFormVisualStates();
        }

        private void KeyboardHook_OnKeyboardEventInterceptor(Keys key, ButtonDisposition keyboardButtonDirection)
        {
            if (key == Keys.F6 && keyboardButtonDirection == ButtonDisposition.Up)
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
            else if (key == Keys.F7 && keyboardButtonDirection == ButtonDisposition.Up)
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

            var item = new ListViewItem(new string[] { recording.Name, recording.SafeDateTimeName });
            listViewHistory.Items.Insert(0, item).Tag = recording;
            item.Checked = true;

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
                    rowPersist.Selected = item.Checked;
                    rowPersist.Name = item.Text;
                    recordings.Add(rowPersist);
                }
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            ApplicationData.SaveToDisk("MacroBot", recordings, settings);
        }

        private void LoadRecordings()
        {
            _isGridPopulating = true;
            var recordings = ApplicationData.LoadFromDisk("MacroBot", new List<PersistedRecording>());

            foreach (var recording in recordings)
            {
                var item = new ListViewItem(new string[] { recording.Name, recording.SafeDateTimeName });
                listViewHistory.Items.Add(item).Tag = recording;
                item.Checked = recording.Selected;
            }
            _isGridPopulating = false;
        }

        private PersistedRecording? GetSelectedRecording()
        {
            foreach (ListViewItem item in listViewHistory.Items)
            {
                if (item.Checked)
                {
                    return (PersistedRecording?)item.Tag;
                }
            }

            return null;
        }

        private void StartPlay()
        {
            var recording = GetSelectedRecording();
            if (recording != null)
            {
                _actionPlayer.Start(recording);
            }
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
