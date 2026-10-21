using MacroBot.Forms;
using MacroBot.Hooks;
using MacroBot.Properties;
using MacroBot.Recording;
using MacroBot.Win32;
using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;
using NTDLS.WinFormsHelpers.Forms;

namespace MacroBot
{
    public partial class FormMain : Form
    {
        private bool _isSettingsDialogOpen;
        private readonly ActionRecorder _actionRecorder = new();
        private readonly ActionPlayer _actionPlayer = new();
        private bool _isGridPopulating = false;
        private readonly ToolTip _toolTip = new();

        public FormMain()
        {
            InitializeComponent();
            Theming.InitializeApplication();
        }

        private void FormMain_Load(object sender, EventArgs e)
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

            InitializeButtonImages();
            InitializeGridContextMenu();

            LoadRecordings();
            ToggleFormVisualStates();
            _actionPlayer.OnStopped += ActionPlayer_OnStopped;

            FormClosed += (object? sender, FormClosedEventArgs e) => SaveRecordings();

            UpdateButtonHints();
        }

        private void InitializeButtonImages()
        {
            var size = (int)(40 * DeviceDpi / 96f);

            buttonPlay.Image = ButtonIcons.Play(size);
            buttonRecord.Image = ButtonIcons.Record(size);
            buttonStop.Image = ButtonIcons.Stop(size);

            buttonPlay.AccessibleName = "Play";
            buttonRecord.AccessibleName = "Record";
            buttonStop.AccessibleName = "Stop";
        }

        private void UpdateButtonHints()
        {
            _toolTip.SetToolTip(buttonPlay, $"Play ({Program.Settings.PlayHotkey})");
            _toolTip.SetToolTip(buttonRecord, $"Record ({Program.Settings.RecordHotkey})");
            _toolTip.SetToolTip(buttonStop, "Stop");
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
                DeleteSelectedRecording();
            }
        }

        private void DeleteSelectedRecording()
        {
            if (listViewHistory.SelectedItems.Count == 0)
            {
                return;
            }

            var item = listViewHistory.SelectedItems[0];

            if (MessageBox.Show($"Delete the recording '{item.Text}'", "MacroBot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            listViewHistory.Items.Remove(item);

            SaveRecordings();
        }

        private void InitializeGridContextMenu()
        {
            var renameMenuItem = new ToolStripMenuItem("Rename", null, (s, e) =>
            {
                if (listViewHistory.SelectedItems.Count > 0)
                {
                    listViewHistory.SelectedItems[0].BeginEdit();
                }
            }) { ShortcutKeyDisplayString = "F2" };

            var deleteMenuItem = new ToolStripMenuItem("Delete", null, (s, e) => DeleteSelectedRecording())
            {
                ShortcutKeyDisplayString = "Del"
            };

            var playMenuItem = new ToolStripMenuItem("Play", null, (s, e) =>
            {
                if (listViewHistory.SelectedItems.Count > 0)
                {
                    //Checking the item makes it the selected recording (the other items are unchecked).
                    listViewHistory.SelectedItems[0].Checked = true;
                    StartPlay();
                }
            });

            var menu = new ContextMenuStrip();
            menu.Items.AddRange([playMenuItem, new ToolStripSeparator(), renameMenuItem, deleteMenuItem]);

            menu.Opening += (s, e) =>
            {
                //Only show the menu when right clicking a recording.
                if (listViewHistory.SelectedItems.Count == 0)
                {
                    e.Cancel = true;
                    return;
                }

                //We cannot play while recording or already playing.
                playMenuItem.Enabled = !_actionRecorder.IsRunning && !_actionPlayer.IsRunning;
            };

            listViewHistory.ContextMenuStrip = menu;
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

        private void KeyboardHook_OnKeyboardEventInterceptor(KeyboardHookEvent e)
        {
            //Input injected by a playing macro must never trigger the hotkeys.
            if (_isSettingsDialogOpen || e.Injected)
            {
                return;
            }

            var key = e.Key;
            var keyboardButtonDirection = e.Disposition;

            if (key == Enum.Parse<Keys>(Program.Settings.RecordHotkey) && keyboardButtonDirection == ButtonDisposition.Up)
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
            else if (key == Enum.Parse<Keys>(Program.Settings.PlayHotkey) && keyboardButtonDirection == ButtonDisposition.Up)
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
            buttonStop.BackColor = _actionRecorder.IsRunning || _actionPlayer.IsRunning ? Color.IndianRed : SystemColors.Control;

            buttonPlay.BackColor = _actionPlayer.IsRunning ? Color.LightGreen : SystemColors.Control;

            buttonRecord.Enabled = !_actionRecorder.IsRunning && !_actionPlayer.IsRunning;
            buttonStop.Enabled = _actionRecorder.IsRunning || _actionPlayer.IsRunning;

            buttonPlay.Enabled = !_actionRecorder.IsRunning && !_actionPlayer.IsRunning;

            if (Program.Settings.HideWhenMinimized && (_actionRecorder.IsRunning || _actionPlayer.IsRunning))
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void StartRecord()
        {
            _actionRecorder.Start(Program.Settings.RelativeMouseMode);
            ToggleFormVisualStates();
        }

        private void StopRecord()
        {
            _actionRecorder.Stop();

            var recording = new PersistedRecording()
            {
                Name = $"Recording {(listViewHistory.Items.Count + 1):n0}",
                CreatedDate = DateTime.Now,
                MouseMode = _actionRecorder.MouseMode,
                Actions = _actionRecorder.Actions
            };

            var item = new ListViewItem(new string[] { recording.Name, recording.MouseMode.ToString(), recording.SafeDateTimeName });
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

            var options = new PersistenceOptions
            {
                Compression = new RmDeflateCompressionProvider()
            };

            LocalUserApplicationData.SaveToDisk("MacroBot", recordings, options);
        }

        private void LoadRecordings()
        {
            var options = new PersistenceOptions
            {
                Compression = new RmDeflateCompressionProvider()
            };

            _isGridPopulating = true;
            List<PersistedRecording> recordings;

            try
            {
                var df = LocalUserApplicationData.Path("MacroBot");
                recordings = LocalUserApplicationData.LoadFromDisk("MacroBot", new List<PersistedRecording>(), options);
            }
            catch
            {
                recordings = new List<PersistedRecording>();
            }

            foreach (var recording in recordings)
            {
                var item = new ListViewItem([recording.Name, recording.MouseMode.ToString(), recording.SafeDateTimeName]);
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

        private void ButtonRecord_Click(object sender, EventArgs e) => StartRecord();
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            if (_actionRecorder.IsRunning)
                StopRecord();
            else if (_actionPlayer.IsRunning)
                StopPlay();
        }
        private void ButtonPlay_Click(object sender, EventArgs e) => StartPlay();

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new FormAbout("MacroBot", this.Icon, Resources.Logo128);
            form.ShowDialog();
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isSettingsDialogOpen = true;
            using var form = new FormSettings();
            form.ShowDialog();

            UpdateButtonHints();
            _isSettingsDialogOpen = false;
        }

        private void NotifyIconMain_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
    }
}
