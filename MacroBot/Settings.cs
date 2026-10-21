namespace MacroBot
{
    internal class Settings
    {
        public bool HideWhenMinimized { get; set; } = true;
        public string RecordHotkey { get; set; } = "F6";
        public string PlayHotkey { get; set; } = "F7";

        /// <summary>A culture name such as "de" or "pt-BR". Empty follows the Windows language.</summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Record relative mouse movement instead of absolute cursor positions. Required for games that capture the mouse.
        /// </summary>
        public bool RelativeMouseMode { get; set; } = false;
    }
}
