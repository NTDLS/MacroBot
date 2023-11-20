namespace MacroBot
{
    internal class PersistedRecording
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<RepeatableAction> Actions { get; set; } = new();

        public string SafeDateTimeName()
        {
            if (CreatedDate == null)
            {
                return "";
            }
            var dateTime = (DateTime)CreatedDate;
            return dateTime.ToShortDateString().Replace("/", ".") + " " + dateTime.ToShortTimeString().Replace(":", ".");
        }
    }
}
