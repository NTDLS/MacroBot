using MacroBot.Win32;
using System.Runtime.InteropServices;
using static MacroBot.Win32.Extern;

namespace MacroBot.Hooks
{
    /// <summary>
    /// Receives relative mouse movement through Raw Input, which is the same source games with a captured mouse read.
    /// Must be started on a thread that pumps messages (the UI thread).
    /// </summary>
    internal class RawMouseInput : NativeWindow
    {
        private const int WM_INPUT = 0x00FF;
        private const uint RID_INPUT = 0x10000003;
        private const uint RIM_TYPEMOUSE = 0;
        private const uint RIDEV_REMOVE = 0x00000001;
        private const uint RIDEV_INPUTSINK = 0x00000100;
        private const ushort MOUSE_MOVE_ABSOLUTE = 0x01;
        private static readonly nint HWND_MESSAGE = -3;

        private Action<int, int>? _onMove;

        public bool Start(Action<int, int> onMove)
        {
            _onMove = onMove;

            CreateHandle(new CreateParams { Parent = HWND_MESSAGE });

            var device = new RAWINPUTDEVICE
            {
                usUsagePage = 0x01, //Generic desktop controls
                usUsage = 0x02, //Mouse
                dwFlags = RIDEV_INPUTSINK, //Receive input even when we are not the foreground window.
                hwndTarget = Handle
            };

            if (!RegisterRawInputDevices([device], 1, (uint)Marshal.SizeOf<RAWINPUTDEVICE>()))
            {
                DestroyHandle();
                _onMove = null;
                return false;
            }
            return true;
        }

        public void Stop()
        {
            if (Handle == nint.Zero)
            {
                return;
            }

            var device = new RAWINPUTDEVICE { usUsagePage = 0x01, usUsage = 0x02, dwFlags = RIDEV_REMOVE, hwndTarget = nint.Zero };
            RegisterRawInputDevices([device], 1, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());

            DestroyHandle();
            _onMove = null;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_INPUT)
            {
                ProcessRawInput(m.LParam);
            }
            base.WndProc(ref m);
        }

        private void ProcessRawInput(nint hRawInput)
        {
            uint headerSize = (uint)Marshal.SizeOf<RAWINPUTHEADER>();
            uint size = 0;
            GetRawInputData(hRawInput, RID_INPUT, nint.Zero, ref size, headerSize);
            if (size == 0)
            {
                return;
            }

            var buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                if (GetRawInputData(hRawInput, RID_INPUT, buffer, ref size, headerSize) != size)
                {
                    return;
                }

                var header = Marshal.PtrToStructure<RAWINPUTHEADER>(buffer);
                if (header.dwType != RIM_TYPEMOUSE)
                {
                    return;
                }

                var mouse = Marshal.PtrToStructure<RAWMOUSE>(buffer + (int)headerSize);
                if ((mouse.usFlags & MOUSE_MOVE_ABSOLUTE) == 0 && (mouse.lLastX != 0 || mouse.lLastY != 0))
                {
                    _onMove?.Invoke(mouse.lLastX, mouse.lLastY);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
