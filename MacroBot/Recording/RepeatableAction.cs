using MacroBot.Win32;
using System.Text.Json.Serialization;

namespace MacroBot.Recording
{
    internal class RepeatableAction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
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

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Keys? Key { get; set; }

        /// <summary>Hardware scan code of the key. Games generally read this rather than the virtual key.</summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ushort? ScanCode { get; set; }

        /// <summary>True when the key is an extended key (arrows, right ctrl/alt, numpad enter, etc.).</summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Extended { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ButtonDisposition? Disposition { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ActionTypes ActionType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MouseX { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MouseY { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? DeltaX { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? DeltaY { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WheelDelta { get; set; }
    }
}
