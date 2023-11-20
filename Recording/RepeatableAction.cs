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
            MouseMove,
            Keyboard
        }

        public int DeltaMilliseconds { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Keys? Key { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ButtonDisposition? Disposition { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ActionTypes ActionType { get; set; }
        public int? MouseX { get; set; }
        public int? MouseY { get; set; }
    }
}
