using CommunityToolkit.Mvvm.Messaging;
using FastEvalCL;
using FastSLNEvaluator2024.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FastSLNEvaluator2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new SolutionsVM();
            this.DataContext = ViewModel ;
        }
        SolutionsVM ViewModel            ;

        private async void Load_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            //TODO: uit settings halen
            //dlg.SelectedPath = AllSettings.LastSelectedFolder;

            if (dlg.ShowDialog() == true)
            {
                //AllSettings.LastSelectedFolder = dlg.SelectedPath;
                // AllSettings.SafeSettings();
                allWindow.IsEnabled = false;
                //  string[] allfiles = Directory.GetFiles(dlg.SelectedPath, "Program.cs", SearchOption.AllDirectories);

                await ViewModel.LoadSolutionsAsync(dlg.SelectedPath);

                allWindow.IsEnabled = true;
            }
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Use me");
        }

        private void TestWelkeCompileren_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ViewModel.Solutions)
            {
                item.TestIfCompiles();
            }
        }
    }
}