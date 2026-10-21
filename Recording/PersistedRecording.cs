using System.Text.Json.Serialization;

namespace MacroBot.Recording
{
    internal enum MouseModes
    {
        /// <summary>Mouse movement is recorded as screen positions.</summary>
        Absolute,
        /// <summary>Mouse movement is recorded as relative deltas (Raw Input), for games with a captured mouse.</summary>
        Relative
    }

    internal class PersistedRecording
    {
        public bool Selected { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// How mouse movement was recorded. This is informational: playback follows the type of each action.
        /// Recordings saved before this property existed are absolute.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MouseModes MouseMode { get; set; } = MouseModes.Absolute;
        public List<RepeatableAction> Actions { get; set; } = new();
        public double Speed { get; set; } = 1.0;
        public int Repetitions { get; set; } = 0;
        public int RepetitionDelay { get; set; } = 1000;

        [JsonIgnore]
        public string SafeDateTimeName
            => CreatedDate.ToShortDateString().Replace("/", ".") + " " + CreatedDate.ToShortTimeString().Replace(":", ".");
    }
}
