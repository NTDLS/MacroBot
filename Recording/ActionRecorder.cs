using MacroBot.Hooks;
using MacroBot.Win32;
using System.Diagnostics;
using static MacroBot.Recording.RepeatableAction;

namespace MacroBot.Recording
{
    internal class ActionRecorder
    {
        /// <summary>
        /// Mouse movement arriving faster than this is merged into a single action.
        /// </summary>
        private const double MinMoveIntervalMs = 4;

        private readonly Stopwatch _clock = new();
        private readonly RawMouseInput _rawMouse = new();
        private readonly Dictionary<Keys, KeyboardHookEvent> _heldKeys = new();
        private readonly HashSet<ActionTypes> _heldButtons = new();

        private double? _lastActionMs;
        private double _lastMoveFlushMs;
        private bool _relativeMouse;
        private Keys? _recordHotkey;
        private Keys? _playHotkey;

        private bool _hasPendingMove;
        private double _pendingMoveMs;
        private int _pendingX;
        private int _pendingY;
        private int _lastX = int.MinValue;
        private int _lastY = int.MinValue;

        public List<RepeatableAction> Actions { get; private set; } = new();
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Starts recording.
        /// </summary>
        /// <param name="relativeMouse">Record relative mouse movement (Raw Input) instead of absolute cursor positions.
        /// This is what games with a captured mouse (first person cameras) need.</param>
        public bool Start(bool relativeMouse)
        {
            if (IsRunning)
            {
                return false;
            }

            Actions = new();
            _heldKeys.Clear();
            _heldButtons.Clear();
            _lastActionMs = null;
            _lastMoveFlushMs = 0;
            _hasPendingMove = false;
            _pendingX = 0;
            _pendingY = 0;
            _lastX = int.MinValue;
            _lastY = int.MinValue;
            _relativeMouse = relativeMouse;

            //The hotkeys that control MacroBot must never end up inside the recording.
            _recordHotkey = Enum.TryParse<Keys>(Program.Settings.RecordHotkey, out var record) ? record : null;
            _playHotkey = Enum.TryParse<Keys>(Program.Settings.PlayHotkey, out var play) ? play : null;

            _clock.Restart();
            IsRunning = true;

            KeyboardHook.OnKeyboardEventInterceptor += OnKeyboardEvent;
            MouseHook.Install(OnMouseEvent);

            if (_relativeMouse && !_rawMouse.Start(OnRawMouseMove))
            {
                //Raw input is unavailable, fall back to absolute positions.
                _relativeMouse = false;
            }

            return true;
        }

        public bool Stop()
        {
            if (!IsRunning)
            {
                return false;
            }
            IsRunning = false;

            KeyboardHook.OnKeyboardEventInterceptor -= OnKeyboardEvent;
            MouseHook.Remove();
            _rawMouse.Stop();

            FlushPendingMove();

            //Anything still held when recording stopped (the key that was held while pressing the stop hotkey)
            //is released so that playback does not leave it stuck down.
            var now = _clock.Elapsed.TotalMilliseconds;

            foreach (var button in _heldButtons)
            {
                AddAction(new RepeatableAction { ActionType = button, Disposition = ButtonDisposition.Up }, now);
            }
            _heldButtons.Clear();

            foreach (var held in _heldKeys.Values)
            {
                AddAction(new RepeatableAction
                {
                    ActionType = ActionTypes.Keyboard,
                    Disposition = ButtonDisposition.Up,
                    Key = held.Key,
                    ScanCode = held.ScanCode,
                    Extended = held.Extended ? true : null
                }, now);
            }
            _heldKeys.Clear();

            _clock.Stop();
            return true;
        }

        private void AddAction(RepeatableAction action, double timestampMs)
        {
            action.DelayMilliseconds = _lastActionMs == null ? 0 : Math.Round(Math.Max(0, timestampMs - _lastActionMs.Value), 3);
            _lastActionMs = timestampMs;
            Actions.Add(action);
        }

        private void OnKeyboardEvent(KeyboardHookEvent e)
        {
            if (e.Injected || e.Key == _recordHotkey || e.Key == _playHotkey)
            {
                return;
            }

            if (e.Disposition == ButtonDisposition.Down)
            {
                //Ignore keyboard auto-repeat, the key is already down.
                if (!_heldKeys.TryAdd(e.Key, e))
                {
                    return;
                }
            }
            else if (!_heldKeys.Remove(e.Key))
            {
                //Released a key that was pressed before recording started.
                return;
            }

            var now = _clock.Elapsed.TotalMilliseconds;
            FlushPendingMove();
            AddAction(new RepeatableAction
            {
                ActionType = ActionTypes.Keyboard,
                Disposition = e.Disposition,
                Key = e.Key,
                ScanCode = e.ScanCode,
                Extended = e.Extended ? true : null
            }, now);
        }

        private void OnMouseEvent(MouseHookEvent e)
        {
            if (e.Injected)
            {
                return;
            }

            var now = _clock.Elapsed.TotalMilliseconds;

            switch (e.Message)
            {
                case MouseMessages.WM_MOUSEMOVE:
                    if (!_relativeMouse)
                    {
                        QueueAbsoluteMove(now, e.X, e.Y);
                    }
                    break;
                case MouseMessages.WM_LBUTTONDOWN:
                    RecordButton(ActionTypes.MouseLeftButton, ButtonDisposition.Down, now);
                    break;
                case MouseMessages.WM_LBUTTONUP:
                    RecordButton(ActionTypes.MouseLeftButton, ButtonDisposition.Up, now);
                    break;
                case MouseMessages.WM_RBUTTONDOWN:
                    RecordButton(ActionTypes.MouseRightButton, ButtonDisposition.Down, now);
                    break;
                case MouseMessages.WM_RBUTTONUP:
                    RecordButton(ActionTypes.MouseRightButton, ButtonDisposition.Up, now);
                    break;
                case MouseMessages.WM_MBUTTONDOWN:
                    RecordButton(ActionTypes.MouseMiddleButton, ButtonDisposition.Down, now);
                    break;
                case MouseMessages.WM_MBUTTONUP:
                    RecordButton(ActionTypes.MouseMiddleButton, ButtonDisposition.Up, now);
                    break;
                case MouseMessages.WM_XBUTTONDOWN:
                    RecordButton(e.Data == 2 ? ActionTypes.MouseXButton2 : ActionTypes.MouseXButton1, ButtonDisposition.Down, now);
                    break;
                case MouseMessages.WM_XBUTTONUP:
                    RecordButton(e.Data == 2 ? ActionTypes.MouseXButton2 : ActionTypes.MouseXButton1, ButtonDisposition.Up, now);
                    break;
                case MouseMessages.WM_MOUSEWHEEL:
                    FlushPendingMove();
                    AddAction(new RepeatableAction { ActionType = ActionTypes.MouseWheel, WheelDelta = e.Data }, now);
                    break;
                case MouseMessages.WM_MOUSEHWHEEL:
                    FlushPendingMove();
                    AddAction(new RepeatableAction { ActionType = ActionTypes.MouseHorizontalWheel, WheelDelta = e.Data }, now);
                    break;
            }
        }

        private void RecordButton(ActionTypes button, ButtonDisposition disposition, double now)
        {
            if (disposition == ButtonDisposition.Down)
            {
                if (!_heldButtons.Add(button))
                {
                    return;
                }
            }
            else if (!_heldButtons.Remove(button))
            {
                //Released a button that was pressed before recording started.
                return;
            }

            FlushPendingMove();
            AddAction(new RepeatableAction { ActionType = button, Disposition = disposition }, now);
        }

        private void QueueAbsoluteMove(double now, int x, int y)
        {
            _pendingX = x;
            _pendingY = y;
            _pendingMoveMs = now;
            _hasPendingMove = true;

            if (now - _lastMoveFlushMs >= MinMoveIntervalMs)
            {
                FlushPendingMove();
            }
        }

        private void OnRawMouseMove(int dx, int dy)
        {
            var now = _clock.Elapsed.TotalMilliseconds;

            //Relative movement is accumulated so that no motion is lost while merging.
            _pendingX += dx;
            _pendingY += dy;
            _pendingMoveMs = now;
            _hasPendingMove = true;

            if (now - _lastMoveFlushMs >= MinMoveIntervalMs)
            {
                FlushPendingMove();
            }
        }

        private void FlushPendingMove()
        {
            if (!_hasPendingMove)
            {
                return;
            }
            _hasPendingMove = false;

            if (_relativeMouse)
            {
                var dx = _pendingX;
                var dy = _pendingY;
                _pendingX = 0;
                _pendingY = 0;

                if (dx != 0 || dy != 0)
                {
                    AddAction(new RepeatableAction { ActionType = ActionTypes.MouseMoveRelative, DeltaX = dx, DeltaY = dy }, _pendingMoveMs);
                    _lastMoveFlushMs = _pendingMoveMs;
                }
            }
            else if (_pendingX != _lastX || _pendingY != _lastY)
            {
                _lastX = _pendingX;
                _lastY = _pendingY;
                AddAction(new RepeatableAction { ActionType = ActionTypes.MouseMove, MouseX = _pendingX, MouseY = _pendingY }, _pendingMoveMs);
                _lastMoveFlushMs = _pendingMoveMs;
            }
        }
    }
}
