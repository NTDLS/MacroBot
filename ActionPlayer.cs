using NTDLS.Persistence;
using static MacroBot.Win32s;

namespace MacroBot
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

        private bool AbortableSleep(int delayMs)
        {
            //delayMs /= 2;

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
                        Win32s.SetCursorPos(action.MouseX, action.MouseY);
                        break;
                    case RepeatableAction.ActionTypes.MouseLeftDown:
                        SendLeftMouseDown();
                        break;
                    case RepeatableAction.ActionTypes.MouseLeftUp:
                        SendLeftMouseUp();
                        break;
                    case RepeatableAction.ActionTypes.MouseRightDown:
                        SendRightMouseDown();
                        break;
                    case RepeatableAction.ActionTypes.MouseRightUp:
                        SendRightMouseUp();
                        break;
                    case RepeatableAction.ActionTypes.KeyDown:

                        SendKey(action.Key, true);
                        break;
                    case RepeatableAction.ActionTypes.KeyUp:
                        SendKey(action.Key, false);
                        break;
                }

                if (i < _recordedActions.Count - 1)
                {
                    AbortableSleep(_recordedActions[i + 1].DeltaMilliseconds);
                }
            }

            IsRunning = false;
            OnStopped?.Invoke(this);
        }
    }
}
