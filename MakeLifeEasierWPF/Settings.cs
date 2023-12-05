using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace MakeLifeEasierWPF
{
    public class Settings: INotifyPropertyChanged //TODO: VM van maken?hate this shit
    {
        public Settings(bool loadFromSettings = true)
        {
            if (loadFromSettings)
            {
                RegionAmount = Properties.Settings.Default.RegionAmount;
                LastSelectedFolder = Properties.Settings.Default.LastPath;
                DevVsPath = Properties.Settings.Default.DevVsPath;
                CompilerDelay = Properties.Settings.Default.CompilerDelay;
                VerbeterSleutelPath = Properties.Settings.Default.VerbeterSleutelPath;
            }
        }

        private string modelOplossing = "";
        public string ModelOplossing
        {
            get
            {
                if(modelOplossing=="")
                {
                    if (VerbeterSleutelPath != "" && File.Exists(VerbeterSleutelPath)) 
                    {
                        modelOplossing = File.ReadAllText(VerbeterSleutelPath);
                    }
                }
                return modelOplossing;
            }
        }
        public string VerbeterSleutelPath { get; set; } = "";
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
            Properties.Settings.Default.VerbeterSleutelPath = VerbeterSleutelPath;
            Properties.Settings.Default.Save();

            OnPropertyChanged("RegionAmount");
            OnPropertyChanged("LastPath");
            OnPropertyChanged("DevVsPath");
            OnPropertyChanged("CompilerDelay");
            OnPropertyChanged("VerbeterSleutelPath");
            OnPropertyChanged("ModelOplossing");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}