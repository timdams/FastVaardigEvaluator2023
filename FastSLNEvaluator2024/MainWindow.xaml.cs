using FastEvalCL;
using FastSLNEvaluator2024.ViewModels;
using ICSharpCode.AvalonEdit;
using Ookii.Dialogs.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace FastSLNEvaluator2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO search box
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new SolutionsVM();
            this.DataContext = ViewModel;
        }
        SolutionsVM ViewModel;

        private async void Load_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            //TODO uit settings halen
            //dlg.SelectedPath = AllSettings.LastSelectedFolder;

            if (dlg.ShowDialog() == true)
            {
                //TODO settings
                //AllSettings.LastSelectedFolder = dlg.SelectedPath;
                // AllSettings.SafeSettings();
                await ProcessSolutions(dlg.SelectedPath);
            }
        }

        private async Task ProcessSolutions(string path)
        {
            allWindow.IsEnabled = false;

            loadingRing.Visibility = Visibility.Visible;
            //TODO projecten in achtergrond beginnen inladen op aparte thread 
            await ViewModel.LoadSolutionsAsync(path);
            loadingRing.Visibility = Visibility.Collapsed;
            allWindow.IsEnabled = true;
            if (ViewModel.Solutions.Count() == 0)
            {
                System.Windows.MessageBox.Show("Geen solutions (*.sln) in folder en subfolders gevonden.");
            }
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("Use me"); //om snel databinding errors te debuggen
        }

        private void TestWelkeCompileren_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Solutions.Count < 5 || System.Windows.MessageBox.Show("Dit zal lang duren. Ben je zeker?", "Opgelet", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                foreach (var item in ViewModel.Solutions)
                {

                    Task.Run(() => item.TestIfCompilesAndRun());
                }
            }
        }

        private void textEditor_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool ctrl = Keyboard.Modifiers == ModifierKeys.Control;
            if (ctrl)
            {
                this.UpdateFontSize(e.Delta > 0, sender as TextEditor);
                e.Handled = true;
            }
        }

        #region avalon zoom bron: https://github.com/icsharpcode/AvalonEdit/issues/143

        // Reasonable max and min font size values
        private const double FONT_MAX_SIZE = 60d;
        private const double FONT_MIN_SIZE = 5d;

        // Update function, increases/decreases by a specific increment
        public void UpdateFontSize(bool increase, TextEditor EditorBox)
        {
            double currentSize = EditorBox.FontSize;

            if (increase)
            {
                if (currentSize < FONT_MAX_SIZE)
                {
                    double newSize = Math.Min(FONT_MAX_SIZE, currentSize + 1);
                    EditorBox.FontSize = newSize;
                }
            }
            else
            {
                if (currentSize > FONT_MIN_SIZE)
                {
                    double newSize = Math.Max(FONT_MIN_SIZE, currentSize - 1);
                    EditorBox.FontSize = newSize;
                }
            }
        }
        #endregion

        private void tryRunProj_Click(object sender, RoutedEventArgs e)
        {
            ((sender as System.Windows.Controls.Button).DataContext as SolutionVM).TryRun();
        }

        private void openExplore_Click(object sender, RoutedEventArgs e)
        {
            ((sender as System.Windows.Controls.Button).DataContext as SolutionVM).OpenInExplorer();
        }

        private void openVS_Click(object sender, RoutedEventArgs e)
        {
            ((sender as System.Windows.Controls.Button).DataContext as SolutionVM).OpenInVS();
        }

        private void txbAutosuggesSearcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.FilterSolution(txbAutosuggesSearcher.Text, SearchCBInFiles.IsChecked.Value);
        }

        private async void MenuItemUnzipMoodleArchive_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog dlg = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dlg.Title = "Kies moodle archive";

            if (dlg.ShowDialog() == true)
            {
                Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dlgSource = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                //TODO beschrijving duidelijker maken
                dlgSource.Description = "Waar moet alles geunzipped worden (maakt een subfolder  op deze plek aan)";
                if (dlgSource.ShowDialog() == true)
                {
                     MoodleOpdrachtUnpacker.UnpackAllOpdrachten(dlg.FileName, dlgSource.SelectedPath);

                    await ProcessSolutions(dlgSource.SelectedPath);
                }
            }
        }
    }
}