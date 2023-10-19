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
            }
        }
        public int RegionAmount { get; internal set; } = 3;
        public string LastSelectedFolder { get; set; } = "C:\\";

        public void SafeSettings()
        {
            Properties.Settings.Default.RegionAmount = RegionAmount;
            Properties.Settings.Default.LastPath = LastSelectedFolder;
            Properties.Settings.Default.Save();
        }
    }
}