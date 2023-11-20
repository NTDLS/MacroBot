using static MacroBot.Win32.Extern;

namespace MacroBot.Recording
{
    internal class ActionPlayer
    {
        private Thread? _thread;

        private PersistedRecording? _recording;

        public bool IsRunning { get; private set; }

        public delegate void StateChanged(ActionPlayer sender);

        public event StateChanged? OnStarted;
        public event StateChanged? OnStopped;

        public bool Start(PersistedRecording recording)
        {
            if (IsRunning)
            {
                return false;
            }

            _recording = recording;

            IsRunning = true;
            OnStarted?.Invoke(this);
            _thread = new Thread(PlayThreadProc);
            _thread.Start();
            return true;
        }

        public bool Stop()
        {
            if (!IsRunning)
            {
                return false;
            }
            IsRunning = false;

            if (_thread == null)
            {
                throw new Exception("The playback thread has not been created.");
            }
            _thread.Join();

            OnStopped?.Invoke(this);

            return true;
        }

        private bool AbortableSleep(int delayMs, double factor)
        {
            delayMs = (int)(delayMs / factor);

            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < delayMs && IsRunning)
            {
                Thread.Sleep(1);
            }

            return IsRunning;
        }

        private void PlayThreadProc()
        {
            if (_recording == null)
            {
                return;
            }

            for (int i = 0; i < _recording.Actions.Count; i++)
            {
                var action = _recording.Actions[i];

                if (IsRunning == false)
                {
                    return;
                }

                switch (action.ActionType)
                {
                    case RepeatableAction.ActionTypes.MouseMove:
                        SetCursorPos(action.MouseX ?? 0, action.MouseY ?? 0);
                        break;
                    case RepeatableAction.ActionTypes.MouseLeftButton:
                        if (action.Disposition == Win32.ButtonDisposition.Down)
                            SendLeftMouseDown();
                        else SendLeftMouseUp();
                        break;
                    case RepeatableAction.ActionTypes.MouseRightButton:
                        if (action.Disposition == Win32.ButtonDisposition.Down)
                            SendRightMouseDown();
                        else SendRightMouseUp();
                        break;
                    case RepeatableAction.ActionTypes.Keyboard:
                        if (action.Key != null)
                        {
                            if (action.Disposition == Win32.ButtonDisposition.Down)
                                SendKey((Keys)action.Key, false);
                            else SendKey((Keys)action.Key, true);
                        }
                        break;
                }

                if (i < _recording.Actions.Count - 1)
                {
                    AbortableSleep(_recording.Actions[i + 1].DelayMilliseconds, _recording.Speed);
                }
            }

            IsRunning = false;
            OnStopped?.Invoke(this);
        }
    }
}
