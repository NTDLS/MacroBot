using MacroBot.Win32;
using static MacroBot.Win32.Extern;

namespace MacroBot.Hooks
{
    internal class MouseHook
    {
        private static readonly LowLevelMouseProc _proc = HookCallback;
        private static nint _hookID = nint.Zero;

        private static MouseEventInterceptor? _handlerProc;

        public static void Install(MouseEventInterceptor handlerProc)
        {
            _hookID = SetHook(_proc);
            _handlerProc = handlerProc;
        }

        public static void Remove()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static nint HookCallback(int nCode, nint wParam, nint lParam)
        {
            if (_handlerProc == null)
            {
                throw new Exception("The callback method has not been defined.");
            }

            if (nCode >= 0 && (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam
                || MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam
                || MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam
                || MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam))
            {
                if (GetCursorPos(out POINT cursorPos))
                {
                    ButtonDisposition direction;

                    MouseButtons buttons;
                    if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                    {
                        buttons = MouseButtons.Left;
                        direction = ButtonDisposition.Down;
                    }
                    else if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                    {
                        buttons = MouseButtons.Right;
                        direction = ButtonDisposition.Down;
                    }
                    else if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
                    {
                        buttons = MouseButtons.Left;
                        direction = ButtonDisposition.Up;
                    }
                    else if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
                    {
                        buttons = MouseButtons.Right;
                        direction = ButtonDisposition.Up;
                    }
                    else
                    {
                        return CallNextHookEx(_hookID, nCode, wParam, lParam);
                    }

                    _handlerProc(new MouseEventArgs(buttons, 1, cursorPos.X, cursorPos.Y, 0), direction);
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
