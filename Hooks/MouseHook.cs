using MacroBot.Win32;
using System.Runtime.InteropServices;
using static MacroBot.Win32.Extern;

namespace MacroBot.Hooks
{
    internal class MouseHook
    {
        private const uint LLMHF_INJECTED = 0x01;

        private static readonly LowLevelMouseProc _proc = HookCallback;
        private static nint _hookID = nint.Zero;

        private static MouseEventInterceptor? _handlerProc;

        public static void Install(MouseEventInterceptor handlerProc)
        {
            _handlerProc = handlerProc;
            _hookID = SetHook(_proc);
        }

        public static void Remove()
        {
            UnhookWindowsHookEx(_hookID);
            _hookID = nint.Zero;
            _handlerProc = null;
        }

        private static nint HookCallback(int nCode, nint wParam, nint lParam)
        {
            if (nCode >= 0 && _handlerProc != null)
            {
                var message = (MouseMessages)wParam;

                switch (message)
                {
                    case MouseMessages.WM_MOUSEMOVE:
                    case MouseMessages.WM_LBUTTONDOWN:
                    case MouseMessages.WM_LBUTTONUP:
                    case MouseMessages.WM_RBUTTONDOWN:
                    case MouseMessages.WM_RBUTTONUP:
                    case MouseMessages.WM_MBUTTONDOWN:
                    case MouseMessages.WM_MBUTTONUP:
                    case MouseMessages.WM_XBUTTONDOWN:
                    case MouseMessages.WM_XBUTTONUP:
                    case MouseMessages.WM_MOUSEWHEEL:
                    case MouseMessages.WM_MOUSEHWHEEL:
                        var info = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);

                        //The high word of mouseData holds the wheel delta (signed) or the X button number.
                        int data = message is MouseMessages.WM_MOUSEWHEEL or MouseMessages.WM_MOUSEHWHEEL
                            ? (short)(info.mouseData >> 16)
                            : (int)(info.mouseData >> 16);

                        _handlerProc(new MouseHookEvent(message, info.pt.X, info.pt.Y, data, (info.flags & LLMHF_INJECTED) != 0));
                        break;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
