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
            var mutex = new Mutex(true, "MacroBot", out bool createdNew);

            try
            {
                if (createdNew == false)
                {
                    MessageBox.Show("Another instance of MacroBot is already running... check the system tray?",
                        "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

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
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}