using NTDLS.Persistence;
using static MacroBot.RepeatableAction;
using static MacroBot.Win32s;

namespace MacroBot
{
    internal class ActionRecorder
    {
        private System.Windows.Forms.Timer _recordTimer = new();
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
                action.DeltaMilliseconds = 0;
            }
            else
            {
                action.DeltaMilliseconds = (int)(DateTime.Now - (DateTime)lastRecordedDateTime).TotalMilliseconds; ;
            }

            Actions.Add(action);
            lastRecordedDateTime = DateTime.Now;

            if (action.ActionType == ActionTypes.MouseMove)
            {
                _lastMouseX = action.MouseX;
                _lastMouseY = action.MouseY;
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
                ActionTypes actionType;

                if (mouseButtonDisposition == ButtonDisposition.Up)
                {
                    actionType = o.Button == MouseButtons.Left ? ActionTypes.MouseLeftUp : ActionTypes.MouseRightUp;
                }
                else if (mouseButtonDisposition == ButtonDisposition.Down)
                {
                    actionType = o.Button == MouseButtons.Left ? ActionTypes.MouseLeftDown : ActionTypes.MouseRightDown;
                }
                else
                {
                    return;
                }

                AddRecordedAction(new RepeatableAction()
                {
                    MouseX = o.X,
                    MouseY = o.Y,
                    ActionType = actionType
                });

                //Debug.WriteLine($"Mouse {mouseButtonDisposition} at: X={o.X}, Y={o.Y}");
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

                    //Debug.WriteLine($"Mouse cursor position: X={cursorPos.X}, Y={cursorPos.Y}");
                }
            };
            _recordTimer.Start();
            return true;
        }

        private void KeyboardHook_OnKeyboardEventInterceptor(Keys key, ButtonDisposition keyboardButtonDirection)
        {
            ActionTypes actionType = keyboardButtonDirection == ButtonDisposition.Up ? ActionTypes.KeyUp : ActionTypes.KeyDown;

            AddRecordedAction(new RepeatableAction()
            {
                Key = key,
                ActionType = actionType
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
