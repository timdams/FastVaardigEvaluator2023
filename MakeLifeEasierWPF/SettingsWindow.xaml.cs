using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MakeLifeEasierWPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(Settings settings)
        {
            AllSettings = settings;
            InitializeComponent();
        }

        private void btnOkSettings_Click(object sender, RoutedEventArgs e)
        {
            //Schrijf settings over

            AllSettings.RegionAmount =Convert.ToInt32(regionAmountSlider.Value);
            AllSettings.CompilerDelay = Convert.ToInt32(compilerDelayText.Text);
            AllSettings.DevVsPath = devvspathText.Text;
            AllSettings.SafeSettings();
            this.Close();
        }
        public Settings AllSettings { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            regionAmountSlider.Value = AllSettings.RegionAmount;
            compilerDelayText.Text = AllSettings.CompilerDelay.ToString();
            devvspathText.Text = AllSettings.DevVsPath;
        }
    }
}
