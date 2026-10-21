using MacroBot.Win32;
using System.Runtime.InteropServices;
using static MacroBot.Win32.Extern;

namespace MacroBot.Hooks
{
    internal class KeyboardHook
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private const uint LLKHF_EXTENDED = 0x01;
        private const uint LLKHF_INJECTED = 0x10;

        private static readonly LowLevelKeyboardProc _proc = HookCallback;
        private static nint _hookID = nint.Zero;
        public static event KeyboardEventInterceptor? OnKeyboardEventInterceptor;

        public static void Install()
        {
            _hookID = SetHook(_proc);
        }

        public static void Remove()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static nint HookCallback(int nCode, nint wParam, nint lParam)
        {
            if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_KEYUP || wParam == WM_SYSKEYDOWN || wParam == WM_SYSKEYUP))
            {
                var info = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

                var keyDisposition = (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) ? ButtonDisposition.Down : ButtonDisposition.Up;

                OnKeyboardEventInterceptor?.Invoke(new KeyboardHookEvent((Keys)info.vkCode, keyDisposition,
                    (ushort)info.scanCode, (info.flags & LLKHF_EXTENDED) != 0, (info.flags & LLKHF_INJECTED) != 0));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
