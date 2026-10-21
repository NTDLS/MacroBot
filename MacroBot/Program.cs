using MacroBot.Hooks;
using NTDLS.Localization;
using NTDLS.Persistence;
using NTDLS.WinFormsHelpers;

namespace MacroBot
{
    internal static class Program
    {
        public static Settings Settings { get; set; } = new();

        [STAThread]
        static void Main()
        {
            var mutex = new Mutex(true, "MacroBot", out bool createdNew);

            Theming.InitializeApplication();

            Settings = LocalUserApplicationData.LoadFromDisk("MacroBot", new Settings());

            //The language is the user's choice in the settings, or else the Windows language.
            Localizer.Default.SetLanguage(Settings.Language);

            try
            {
                if (createdNew == false)
                {
                    MessageBox.Show(Localize("Another instance of MacroBot is already running... check the system tray?"),
                        "MacroBot", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ApplicationConfiguration.Initialize();

                try
                {
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