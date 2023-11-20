namespace MacroBot
{
    internal class RepeatableAction
    {
        public enum ActionTypes
        {
            MouseMove,
            MouseRightDown,
            MouseLeftDown,
            MouseRightUp,
            MouseLeftUp,
            KeyDown,
            KeyUp
        }

        public int DeltaMilliseconds { get; set; }
        public Keys Key { get; set; }
        public ActionTypes ActionType { get; set; }
        public int MouseX { get; set; }
        public int MouseY { get; set; }
    }
}
