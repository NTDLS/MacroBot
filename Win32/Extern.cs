using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacroBot.Win32
{
    internal static class Extern
    {
        public delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);
        public delegate void MouseEventInterceptor(MouseEventArgs e, ButtonDisposition mouseButtonDisposition);
        public delegate void KeyboardEventInterceptor(Keys key, ButtonDisposition keyboardButtonDirection);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, nint hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(nint hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint GetModuleHandle(string lpModuleName);

        public delegate nint LowLevelMouseProc(int nCode, nint wParam, nint lParam);

        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        public static nint SetHook(LowLevelMouseProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            var mainMoudle = curProcess.MainModule ?? throw new Exception("Unable to obtain the main module for the associated process");
            using ProcessModule curModule = mainMoudle;
            return SetWindowsHookEx((int)HookType.WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        public static nint SetHook(LowLevelKeyboardProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            var mainMoudle = curProcess.MainModule ?? throw new Exception("Unable to obtain the main module for the associated process");
            using ProcessModule curModule = mainMoudle;
            return SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        public static void SendKey(Keys key, bool keyUp)
        {
            var inputs = new INPUT[1];
            inputs[0].type = (int)InputType.INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = (ushort)key;
            inputs[0].u.ki.dwFlags = keyUp ? (uint)KeyboardInputFlags.KEYEVENTF_KEYUP : 0;
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendFullMouseClick()
        {
            var inputs = new INPUT[2];

            // Mouse down
            inputs[0].type = (int)InputType.INPUT_MOUSE;
            inputs[0].u.mi.dwFlags = (int)MouseEventFlags.MOUSEEVENTF_LEFTDOWN;

            // Mouse up
            inputs[1].type = (int)InputType.INPUT_MOUSE;
            inputs[1].u.mi.dwFlags = (int)MouseEventFlags.MOUSEEVENTF_LEFTUP;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendLeftMouseDown()
        {
            var inputs = new INPUT[1];
            inputs[0].type = (int)InputType.INPUT_MOUSE;
            inputs[0].u.mi.dwFlags = (int)MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendRightMouseDown()
        {
            var inputs = new INPUT[1];
            inputs[0].type = (int)InputType.INPUT_MOUSE;
            inputs[0].u.mi.dwFlags = (int)MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendLeftMouseUp()
        {
            var inputs = new INPUT[1];
            inputs[0].type = (int)InputType.INPUT_MOUSE;
            inputs[0].u.mi.dwFlags = (int)MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendRightMouseUp()
        {
            var inputs = new INPUT[1];
            inputs[0].type = (int)InputType.INPUT_MOUSE;
            inputs[0].u.mi.dwFlags = (int)MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}
