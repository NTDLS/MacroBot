using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacroBot.Win32
{
    internal static class Extern
    {
        public delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);
        public delegate nint LowLevelMouseProc(int nCode, nint wParam, nint lParam);
        public delegate void MouseEventInterceptor(MouseHookEvent e);
        public delegate void KeyboardEventInterceptor(KeyboardHookEvent e);

        private const uint MAPVK_VK_TO_VSC_EX = 4;
        private const int SM_XVIRTUALSCREEN = 76;
        private const int SM_YVIRTUALSCREEN = 77;
        private const int SM_CXVIRTUALSCREEN = 78;
        private const int SM_CYVIRTUALSCREEN = 79;
        private const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, nint hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(nint hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetRawInputData(nint hRawInput, uint uiCommand, nint pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("winmm.dll")]
        public static extern uint timeBeginPeriod(uint uPeriod);

        [DllImport("winmm.dll")]
        public static extern uint timeEndPeriod(uint uPeriod);

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

        private static void Send(INPUT input)
            => SendInput(1, [input], Marshal.SizeOf<INPUT>());

        /// <summary>
        /// Sends a key press or release. Games that use DirectInput or Raw Input read scan codes rather than
        /// virtual keys, so the key is sent as a scan code. If the scan code was not recorded it is looked up.
        /// </summary>
        public static void SendKey(Keys key, ushort? scanCode, bool? extended, bool keyUp)
        {
            ushort scan = scanCode ?? 0;
            bool isExtended = extended ?? false;

            if (scan == 0)
            {
                var mapped = MapVirtualKey((uint)key, MAPVK_VK_TO_VSC_EX);
                scan = (ushort)(mapped & 0xFF);
                isExtended = (mapped >> 8) is 0xE0 or 0xE1;
            }

            var input = new INPUT { type = (uint)InputType.INPUT_KEYBOARD };
            input.u.ki.wVk = (ushort)key;
            input.u.ki.wScan = scan;

            uint flags = keyUp ? (uint)KeyboardInputFlags.KEYEVENTF_KEYUP : 0;
            if (scan != 0)
            {
                flags |= (uint)KeyboardInputFlags.KEYEVENTF_SCANCODE;
                if (isExtended)
                {
                    flags |= (uint)KeyboardInputFlags.KEYEVENTF_EXTENDEDKEY;
                }
            }
            input.u.ki.dwFlags = flags;

            Send(input);
        }

        /// <summary>
        /// Moves the cursor to an absolute screen position (virtual desktop pixels).
        /// </summary>
        public static void SendMouseMoveAbsolute(int x, int y)
        {
            int left = GetSystemMetrics(SM_XVIRTUALSCREEN);
            int top = GetSystemMetrics(SM_YVIRTUALSCREEN);
            int width = Math.Max(1, GetSystemMetrics(SM_CXVIRTUALSCREEN));
            int height = Math.Max(1, GetSystemMetrics(SM_CYVIRTUALSCREEN));

            int nx = (int)Math.Clamp(((long)(x - left) * 65536 + width - 1) / width, 0, 65535);
            int ny = (int)Math.Clamp(((long)(y - top) * 65536 + height - 1) / height, 0, 65535);

            SendMouse((uint)MouseEventFlags.MOUSEEVENTF_MOVE | (uint)MouseEventFlags.MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_VIRTUALDESK, nx, ny, 0);
        }

        /// <summary>
        /// Moves the mouse by a relative amount, which is what games with a captured mouse read.
        /// </summary>
        public static void SendMouseMoveRelative(int dx, int dy)
            => SendMouse((uint)MouseEventFlags.MOUSEEVENTF_MOVE, dx, dy, 0);

        public static void SendMouse(uint flags, int dx, int dy, uint data)
        {
            var input = new INPUT { type = (uint)InputType.INPUT_MOUSE };
            input.u.mi.dx = dx;
            input.u.mi.dy = dy;
            input.u.mi.mouseData = data;
            input.u.mi.dwFlags = flags;
            Send(input);
        }
    }
}
