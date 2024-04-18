using MacroBot.Hooks;
using NTDLS.Persistence;

namespace MacroBot
{
    internal static class Program
    {
        public static Settings Settings { get; set; } = new();

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                Settings = LocalUserApplicationData.LoadFromDisk("MacroBot", new Settings());

                KeyboardHook.Install();

                Application.Run(new FormMain());
            }
            finally
            {
                KeyboardHook.Remove();
            }
        }
    }
}