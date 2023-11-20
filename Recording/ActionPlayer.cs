using static MacroBot.Win32.Extern;

namespace MacroBot.Recording
{
    internal class ActionPlayer
    {
        private Thread? _thread;
        private List<RepeatableAction> _recordedActions = new();
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

            _recordedActions = recording.Actions;

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
            for (int i = 0; i < _recordedActions.Count; i++)
            {
                var action = _recordedActions[i];

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

                if (i < _recordedActions.Count - 1)
                {
                    AbortableSleep(_recordedActions[i + 1].DeltaMilliseconds, 50);
                }
            }

            IsRunning = false;
            OnStopped?.Invoke(this);
        }
    }
}
