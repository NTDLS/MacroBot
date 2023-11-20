﻿using Newtonsoft.Json;

namespace MacroBot.Recording
{
    internal class PersistedRecording
    {
        public bool Selected { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<RepeatableAction> Actions { get; set; } = new();
        public double Speed { get; set; } = 1.0;

        [JsonIgnore]
        public string SafeDateTimeName
            => CreatedDate.ToShortDateString().Replace("/", ".") + " " + CreatedDate.ToShortTimeString().Replace(":", ".");
    }
}