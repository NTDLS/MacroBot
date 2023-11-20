using MacroBot.Hooks;
using MacroBot.Win32;
using static MacroBot.Recording.RepeatableAction;
using static MacroBot.Win32.Extern;

namespace MacroBot.Recording
{
    internal class ActionRecorder
    {
        private readonly System.Windows.Forms.Timer _recordTimer = new();
        private int _lastMouseX = -1;
        private int _lastMouseY = -1;
        private DateTime? lastRecordedDateTime = null;

        public List<RepeatableAction> Actions { get; private set; } = new();
        public bool IsRunning { get; private set; }

        public void AddRecordedAction(RepeatableAction action)
        {
            if (action.ActionType == ActionTypes.MouseMove)
            {
                if (action.MouseX == _lastMouseX && action.MouseY == _lastMouseY)
                {
                    return;
                }
            }

            if (lastRecordedDateTime == null)
            {
                action.DelayMilliseconds = 0;
            }
            else
            {
                action.DelayMilliseconds = (int)(DateTime.Now - (DateTime)lastRecordedDateTime).TotalMilliseconds; ;
            }

            Actions.Add(action);
            lastRecordedDateTime = DateTime.Now;

            if (action.ActionType == ActionTypes.MouseMove)
            {
                _lastMouseX = action.MouseX ?? 0;
                _lastMouseY = action.MouseY ?? 0;
            }
        }

        public bool Start()
        {
            if (IsRunning)
            {
                return false;
            }
            IsRunning = true;

            _lastMouseX = -1;
            _lastMouseY = -1;
            lastRecordedDateTime = null;
            Actions = new();

            KeyboardHook.OnKeyboardEventInterceptor += KeyboardHook_OnKeyboardEventInterceptor;

            MouseHook.Install((o, mouseButtonDisposition) =>
            {
                AddRecordedAction(new RepeatableAction()
                {
                    Disposition = mouseButtonDisposition,
                    ActionType = o.Button == MouseButtons.Left ? ActionTypes.MouseLeftButton : ActionTypes.MouseRightButton
                });
            });

            _recordTimer.Interval = 10;
            _recordTimer.Tick += (sender, e) =>
            {
                POINT cursorPos;
                if (GetCursorPos(out cursorPos))
                {
                    AddRecordedAction(new RepeatableAction()
                    {
                        MouseX = cursorPos.X,
                        MouseY = cursorPos.Y,
                        ActionType = ActionTypes.MouseMove
                    });
                }
            };
            _recordTimer.Start();
            return true;
        }

        private void KeyboardHook_OnKeyboardEventInterceptor(Keys key, ButtonDisposition keyboardButtonDirection)
        {
            AddRecordedAction(new RepeatableAction()
            {
                Disposition = keyboardButtonDirection,
                Key = key,
                ActionType = ActionTypes.Keyboard
            });
        }

        public bool Stop()
        {
            if (!IsRunning)
            {
                return false;
            }
            IsRunning = false;
            KeyboardHook.OnKeyboardEventInterceptor -= KeyboardHook_OnKeyboardEventInterceptor;
            MouseHook.Remove();
            _recordTimer.Stop();
            return false;
        }
    }
}
