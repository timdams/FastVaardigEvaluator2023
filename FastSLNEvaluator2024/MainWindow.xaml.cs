using FastSLNEvaluator2024.ViewModels;
using ICSharpCode.AvalonEdit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                allWindow.IsEnabled = false;

                loadingRing.Visibility = Visibility.Visible;
                //TODO projecten in achtergrond beginnen inladen op aparte thread 
                await ViewModel.LoadSolutionsAsync(dlg.SelectedPath);
                loadingRing.Visibility = Visibility.Collapsed;
                allWindow.IsEnabled = true;
                if (ViewModel.Solutions.Count() == 0)
                {
                    MessageBox.Show("Geen solutions (*.sln) in folder en subfolders gevonden.");
                }
            }
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Use me"); //om snel databinding errors te debuggen
        }

        private void TestWelkeCompileren_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Dit zal lang duren. Ben je zeker?", "Opgelet", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //TODO backgroundworker needs a job?! This is definitely one!
                allWindow.IsEnabled = false;

                loadingRing.Visibility = Visibility.Visible;
                foreach (var item in ViewModel.Solutions)
                {
                    item.TestIfCompiles();
                }
                loadingRing.Visibility = Visibility.Collapsed;
                allWindow.IsEnabled = true;
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
            ((sender as Button).DataContext as SolutionVM).TryRun();
        }

        private void openExplore_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Button).DataContext as SolutionVM).OpenInExplorer();
        }

        private void openVS_Click(object sender, RoutedEventArgs e)
        {
            ((sender as Button).DataContext as SolutionVM).OpenInVS();
        }
    }
}