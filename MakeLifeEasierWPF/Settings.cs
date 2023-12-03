namespace MakeLifeEasierWPF
{
    public class Settings
    {
        public Settings(bool loadFromSettings = true)
        {
            if (loadFromSettings)
            {
                RegionAmount = Properties.Settings.Default.RegionAmount;
                LastSelectedFolder = Properties.Settings.Default.LastPath;
                DevVsPath = Properties.Settings.Default.DevVsPath;
                CompilerDelay = Properties.Settings.Default.CompilerDelay;
            }
        }
        public int RegionAmount { get; internal set; } = 3;
        public string LastSelectedFolder { get; set; } = "C:\\";
        public int CompilerDelay { get; set; }
        public string DevVsPath { get; set; }

        public void SafeSettings()
        {
            Properties.Settings.Default.RegionAmount = RegionAmount;
            Properties.Settings.Default.LastPath = LastSelectedFolder;
            Properties.Settings.Default.DevVsPath = DevVsPath;
            Properties.Settings.Default.CompilerDelay = CompilerDelay;
            Properties.Settings.Default.Save();
        }
    }
}