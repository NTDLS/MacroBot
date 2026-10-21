using MacroBot.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MacroBot.Recording
{
    internal class RepeatableAction
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ActionTypes
        {
            MouseLeftButton,
            MouseRightButton,
            MouseMiddleButton,
            MouseXButton1,
            MouseXButton2,
            /// <summary>Absolute cursor position (MouseX, MouseY).</summary>
            MouseMove,
            /// <summary>Relative movement (DeltaX, DeltaY), as read by games with a captured mouse.</summary>
            MouseMoveRelative,
            MouseWheel,
            MouseHorizontalWheel,
            Keyboard
        }

        /// <summary>
        /// Time to wait after the previous action before performing this one.
        /// </summary>
        public double DelayMilliseconds { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys? Key { get; set; }

        /// <summary>Hardware scan code of the key. Games generally read this rather than the virtual key.</summary>
        public ushort? ScanCode { get; set; }

        /// <summary>True when the key is an extended key (arrows, right ctrl/alt, numpad enter, etc.).</summary>
        public bool? Extended { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ButtonDisposition? Disposition { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ActionTypes ActionType { get; set; }
        public int? MouseX { get; set; }
        public int? MouseY { get; set; }
        public int? DeltaX { get; set; }
        public int? DeltaY { get; set; }
        public int? WheelDelta { get; set; }
    }
}
