using Newtonsoft.Json;

namespace MacroBot.Recording
{
    internal class PersistedRecording
    {
        public bool Selected { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<RepeatableAction> Actions { get; set; } = new();
        public double Speed { get; set; } = 1.0;
        public int Repetitions { get; set; } = 0;
        public int RepetitionDelay { get; set; } = 1000;
        [JsonIgnore]
        public string SafeDateTimeName
            => CreatedDate.ToShortDateString().Replace("/", ".") + " " + CreatedDate.ToShortTimeString().Replace(":", ".");
    }
}
