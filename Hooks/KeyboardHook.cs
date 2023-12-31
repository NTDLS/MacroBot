﻿using MacroBot.Win32;
using System.Runtime.InteropServices;
using static MacroBot.Win32.Extern;

namespace MacroBot.Hooks
{
    internal class KeyboardHook
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private static LowLevelKeyboardProc _proc = HookCallback;
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
            if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lParam);

                var keyDisposition = wParam == WM_KEYDOWN ? ButtonDisposition.Down : ButtonDisposition.Up;

                OnKeyboardEventInterceptor?.Invoke((Keys)vkCode, keyDisposition);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
