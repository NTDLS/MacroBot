using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacroBot
{
    internal static class Win32s
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void MouseEventInterceptor(MouseEventArgs e, ButtonDisposition mouseButtonDisposition);
        public delegate void KeyboardEventInterceptor(Keys key, ButtonDisposition keyboardButtonDirection);

        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        public enum InputType
        {
            /// <summary>
            /// The event is a mouse event. Use the mi structure of the union.
            /// </summary>
            INPUT_MOUSE = 0,
            /// <summary>
            /// The event is a keyboard event. Use the ki structure of the union.
            /// </summary>
            INPUT_KEYBOARD = 1,
            /// <summary>
            /// The event is a hardware event. Use the hi structure of the union.
            /// </summary>
            INPUT_HARDWARE = 2
        }

        public enum KeyboardInputFlags
        {
            /// <summary>
            /// If specified, the wScan scan code consists of a sequence of two bytes, where the first byte has a value of 0xE0. See Extended-Key Flag for more info.
            /// </summary>
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            /// <summary>
            /// If specified, the key is being released. If not specified, the key is being pressed.
            /// </summary>
            KEYEVENTF_KEYUP = 0x0002,
            /// <summary>
            /// If specified, wScan identifies the key and wVk is ignored.
            /// </summary>
            KEYEVENTF_SCANCODE = 0x0008,
            /// <summary>
            /// If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section.
            /// </summary>
            KEYEVENTF_UNICODE = 0x0004,

        }
        public enum HookType
        {
            WH_MOUSE_LL = 14,
            WH_KEYBOARD_LL = 13
        }
        public static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                var mainMoudle = curProcess.MainModule;
                if (mainMoudle == null)
                {
                    throw new Exception("Unable to obtain the main module for the associated process");
                }

                using (ProcessModule curModule = mainMoudle)
                {
                    return SetWindowsHookEx((int)HookType.WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                var mainMoudle = curProcess.MainModule;
                if (mainMoudle == null)
                {
                    throw new Exception("Unable to obtain the main module for the associated process");
                }

                using (ProcessModule curModule = mainMoudle)
                {
                    return SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        public static void SendKey(Keys key, bool keyUp)
        {
            INPUT[] inputs = new INPUT[1];
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

        public enum MouseEventFlags
        {
            /// <summary>
            /// The dx and dy parameters contain normalized absolute coordinates. If not set, those parameters contain relative data: the change in position since the last reported position. This flag can be set, or not set, regardless of what kind of mouse or mouse-like device, if any, is connected to the system. For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000,
            /// <summary>
            /// The left button is down.
            /// </summary>
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            /// <summary>
            /// The left button is up.
            /// </summary>
            MOUSEEVENTF_LEFTUP = 0x0004,
            /// <summary>
            /// The middle button is down.
            /// </summary>
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            /// <summary>
            /// The middle button is up.
            /// </summary>
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            /// <summary>
            /// Movement occurred.
            /// </summary>
            MOUSEEVENTF_MOVE = 0x0001,
            /// <summary>
            /// The right button is down.
            /// </summary>
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            /// <summary>
            /// The right button is up.
            /// </summary>
            MOUSEEVENTF_RIGHTUP = 0x0010,
            /// <summary>
            /// The wheel has been moved, if the mouse has a wheel. The amount of movement is specified in dwData
            /// </summary>
            MOUSEEVENTF_WHEEL = 0x0800,
            /// <summary>
            /// An X button was pressed.
            /// </summary>
            MOUSEEVENTF_XDOWN = 0x0080,
            /// <summary>
            /// An X button was released.
            /// </summary>
            MOUSEEVENTF_XUP = 0x0100,
            /// <summary>
            /// The wheel button is tilted.
            /// </summary>
            MOUSEEVENTF_HWHEEL = 0x01000
        }


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public enum ButtonDisposition
        {
            Up,
            Down
        }

        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_RBUTTONDOWN = 0x0204,
            WM_LBUTTONUP = 0x0202,
            WM_RBUTTONUP = 0x0205
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
