using MacroBot.Win32;
using System.Diagnostics;
using static MacroBot.Recording.RepeatableAction;
using static MacroBot.Win32.Extern;

namespace MacroBot.Recording
{
    internal class ActionPlayer
    {
        private const uint XBUTTON1 = 1;
        private const uint XBUTTON2 = 2;

        private Thread? _thread;
        private PersistedRecording? _recording;
        private int _isRunning;

        //Inputs we have pressed and not yet released, so that they can be released if playback is stopped.
        private readonly Dictionary<Keys, RepeatableAction> _heldKeys = new();
        private readonly HashSet<ActionTypes> _heldButtons = new();

        public bool IsRunning => Volatile.Read(ref _isRunning) == 1;

        public delegate void StateChanged(ActionPlayer sender);

        public event StateChanged? OnStarted;
        public event StateChanged? OnStopped;

        public bool Start(PersistedRecording recording)
        {
            if (Interlocked.CompareExchange(ref _isRunning, 1, 0) != 0)
            {
                return false;
            }

            _recording = recording;

            OnStarted?.Invoke(this);
            _thread = new Thread(PlayThreadProc) { IsBackground = true };
            _thread.Start();
            return true;
        }

        public bool Stop()
        {
            if (Interlocked.Exchange(ref _isRunning, 0) == 0)
            {
                return false;
            }

            if (_thread == null)
            {
                throw new Exception("The playback thread has not been created.");
            }
            _thread.Join();

            OnStopped?.Invoke(this);

            return true;
        }

        /// <summary>
        /// Waits until the stopwatch reaches the given time. Sleeps while far away and spins for the final stretch,
        /// so that the many small delays between mouse movements are not stretched by the scheduler's granularity.
        /// </summary>
        private bool WaitUntil(Stopwatch clock, double targetMs)
        {
            while (IsRunning)
            {
                var remaining = targetMs - clock.Elapsed.TotalMilliseconds;
                if (remaining <= 0)
                {
                    return true;
                }

                if (remaining > 2)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    Thread.SpinWait(20);
                }
            }

            return false;
        }

        private void PlayThreadProc()
        {
            //Raises the system timer resolution so that Thread.Sleep(1) really sleeps for about 1ms.
            timeBeginPeriod(1);
            try
            {
                Play();
            }
            finally
            {
                ReleaseHeldInputs();
                timeEndPeriod(1);
            }

            //Only notify if we finished on our own, Stop() notifies when it ended playback.
            if (Interlocked.Exchange(ref _isRunning, 0) == 1)
            {
                OnStopped?.Invoke(this);
            }
        }

        private void Play()
        {
            if (_recording == null)
            {
                return;
            }

            var speed = _recording.Speed > 0 ? _recording.Speed : 1.0;
            var clock = new Stopwatch();

            for (int repetition = 0; repetition < _recording.Repetitions || _recording.Repetitions == 0; repetition++)
            {
                //Actions are scheduled against absolute times so that the time spent performing
                //an action does not accumulate as drift.
                double scheduledMs = 0;
                clock.Restart();

                for (int i = 0; i < _recording.Actions.Count; i++)
                {
                    var action = _recording.Actions[i];

                    if (i > 0)
                    {
                        scheduledMs += action.DelayMilliseconds / speed;
                    }

                    if (!WaitUntil(clock, scheduledMs))
                    {
                        return;
                    }

                    Perform(action);
                }

                if (_recording.Repetitions > 0)
                {
                    clock.Restart();
                    if (!WaitUntil(clock, _recording.RepetitionDelay))
                    {
                        return;
                    }
                }
            }
        }

        private void Perform(RepeatableAction action)
        {
            var isDown = action.Disposition == ButtonDisposition.Down;

            switch (action.ActionType)
            {
                case ActionTypes.MouseMove:
                    SendMouseMoveAbsolute(action.MouseX ?? 0, action.MouseY ?? 0);
                    break;
                case ActionTypes.MouseMoveRelative:
                    SendMouseMoveRelative(action.DeltaX ?? 0, action.DeltaY ?? 0);
                    break;
                case ActionTypes.MouseWheel:
                    SendMouse((uint)MouseEventFlags.MOUSEEVENTF_WHEEL, 0, 0, (uint)(action.WheelDelta ?? 0));
                    break;
                case ActionTypes.MouseHorizontalWheel:
                    SendMouse((uint)MouseEventFlags.MOUSEEVENTF_HWHEEL, 0, 0, (uint)(action.WheelDelta ?? 0));
                    break;
                case ActionTypes.MouseLeftButton:
                case ActionTypes.MouseRightButton:
                case ActionTypes.MouseMiddleButton:
                case ActionTypes.MouseXButton1:
                case ActionTypes.MouseXButton2:
                    SendButton(action.ActionType, isDown);
                    if (isDown) _heldButtons.Add(action.ActionType);
                    else _heldButtons.Remove(action.ActionType);
                    break;
                case ActionTypes.Keyboard:
                    if (action.Key != null)
                    {
                        SendKey(action.Key.Value, action.ScanCode, action.Extended, !isDown);
                        if (isDown) _heldKeys[action.Key.Value] = action;
                        else _heldKeys.Remove(action.Key.Value);
                    }
                    break;
            }
        }

        private static void SendButton(ActionTypes button, bool isDown)
        {
            switch (button)
            {
                case ActionTypes.MouseLeftButton:
                    SendMouse((uint)(isDown ? MouseEventFlags.MOUSEEVENTF_LEFTDOWN : MouseEventFlags.MOUSEEVENTF_LEFTUP), 0, 0, 0);
                    break;
                case ActionTypes.MouseRightButton:
                    SendMouse((uint)(isDown ? MouseEventFlags.MOUSEEVENTF_RIGHTDOWN : MouseEventFlags.MOUSEEVENTF_RIGHTUP), 0, 0, 0);
                    break;
                case ActionTypes.MouseMiddleButton:
                    SendMouse((uint)(isDown ? MouseEventFlags.MOUSEEVENTF_MIDDLEDOWN : MouseEventFlags.MOUSEEVENTF_MIDDLEUP), 0, 0, 0);
                    break;
                case ActionTypes.MouseXButton1:
                    SendMouse((uint)(isDown ? MouseEventFlags.MOUSEEVENTF_XDOWN : MouseEventFlags.MOUSEEVENTF_XUP), 0, 0, XBUTTON1);
                    break;
                case ActionTypes.MouseXButton2:
                    SendMouse((uint)(isDown ? MouseEventFlags.MOUSEEVENTF_XDOWN : MouseEventFlags.MOUSEEVENTF_XUP), 0, 0, XBUTTON2);
                    break;
            }
        }

        private void ReleaseHeldInputs()
        {
            foreach (var button in _heldButtons)
            {
                SendButton(button, false);
            }
            _heldButtons.Clear();

            foreach (var held in _heldKeys.Values)
            {
                SendKey(held.Key!.Value, held.ScanCode, held.Extended, true);
            }
            _heldKeys.Clear();
        }
    }
}
