using System.Runtime.InteropServices;

namespace MacroBot.Win32
{
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
}

