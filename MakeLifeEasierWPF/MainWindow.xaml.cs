using FastEvalCL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MakeLifeEasierWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Settings AllSettings { get; set; }
        public ToetsTemplate huidigeTemplate { get; set; }
        public MainWindow()
        {

            InitializeComponent();
        }
        IOrderedEnumerable<SolutionModel> allFiles = null;
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            //TODO: werkt niet?
            dlg.SelectedPath = AllSettings.LastSelectedFolder;

            if (dlg.ShowDialog() == true)
            {
                AllSettings.LastSelectedFolder = dlg.SelectedPath;
                AllSettings.SafeSettings();

                //  string[] allfiles = Directory.GetFiles(dlg.SelectedPath, "Program.cs", SearchOption.AllDirectories);
                allFiles = SolutionHelper.LoadAllSolutionsFromPath(dlg.SelectedPath).OrderBy(p => p.Info.SorteerNaam);
                folderList.ItemsSource = allFiles;
            }
        }

        private void folderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (folderList.SelectedItem != null)
            {
                var activeSolVM = (folderList.SelectedItem as SolutionModel);
                selInfo.DataContext = activeSolVM.Info;
                boeteHeader.DataContext = activeSolVM.Boete;
                txbCompileErrors.DataContext = activeSolVM;
                if (cmbCodeFilter.SelectedIndex == 0)
                    textEditor.Text = File.ReadAllText(activeSolVM.Path);
                else
                {
                    var regions = FastEvalCL.FastEvalCL.GetCodeRegions(File.ReadAllText(activeSolVM.Path));
                    if (regions.Count > cmbCodeFilter.SelectedIndex - 1)
                    {
                        textEditor.Text = regions[cmbCodeFilter.SelectedIndex - 1];
                    }
                    else
                    {
                        textEditor.Text = "********DEZE REGIO NIET GEVONDEN.HIERONDER ALLE CODE: ****\n\r" + File.ReadAllText(activeSolVM.Path); ;
                    }
                }
            }
            else
            {
                selInfo.DataContext = null;
                textEditor.Text = "";
            }

        }

        private void cmbCodeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //1'm @ 4ack3r
            if (folderList.SelectedIndex >= 0)
                folderList_SelectionChanged(this, null);
        }

        private void btnBoeteCheck_Click(object sender, RoutedEventArgs e)
        {
            if (folderList.SelectedItem != null)
            {
                // FastEvalCL.FastEvalCL.BoeteControle(folderList.SelectedItem.ToString());
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow(AllSettings);

            settings.ShowDialog();
            ReloadUI();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllSettings = new Settings();
            modelOplossing.Text = AllSettings.ModelOplossing;
            //TODO  settings wegschrijven/inladen
            ReloadUI();

        }

        private void ReloadUI()
        {
            modelOplossing.Text = AllSettings.ModelOplossing;
            //Filter combobox vullen met juiste hoeveelheid regions
            cmbCodeFilter.SelectionChanged -= cmbCodeFilter_SelectionChanged;
            cmbCodeFilter.Items.Clear();
            cmbCodeFilter.Items.Add(new TextBlock() { Text = "Alles" });
            for (int i = 0; i < AllSettings.RegionAmount; i++)
            {
                cmbCodeFilter.Items.Add(new TextBlock() { Text = "Region " + (i + 1) });
            }
            cmbCodeFilter.SelectionChanged += cmbCodeFilter_SelectionChanged;
            cmbCodeFilter.SelectedIndex = 0;

        }

        private void btnSaveBoetes_Click(object sender, RoutedEventArgs e)
        {
            if (folderList.ItemsSource != null)
            {
                var lijstje = folderList.ItemsSource as IOrderedEnumerable<SolutionModel>;
                foreach (var sol in lijstje)
                {
                    sol.SafeBoete();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var msgResult = MessageBox.Show("Wil je ingevoerde boetes nog wegschrijven?", "OPGELET", MessageBoxButton.YesNoCancel);
            if (msgResult == MessageBoxResult.Yes)
            {
                if (folderList.ItemsSource != null)
                {
                    var lijstje = folderList.ItemsSource as IOrderedEnumerable<SolutionModel>;
                    foreach (var sol in lijstje)
                    {
                        sol.SafeBoete();
                    }
                }
            }
            else if (msgResult == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void btnExportBoetes_Click(object sender, RoutedEventArgs e)
        {
            if (folderList.ItemsSource != null)
            {
                var dlg = new Ookii.Dialogs.Wpf.VistaSaveFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    string res = "achternaam;voornaam;" + Boete.GetCVSLineHeader() + Environment.NewLine;

                    var lijstje = folderList.ItemsSource as IOrderedEnumerable<SolutionModel>;
                    foreach (var sol in lijstje)
                    {
                        res += sol.Info.AchterNaam + ";";
                        res += sol.Info.VoorNaam + ";";
                        res += sol.Boete.GetCVSLine() + Environment.NewLine;
                    }

                    File.WriteAllText(dlg.FileName, res);
                }
            }
        }

        private void btnOpenInExplorer_Click(object sender, RoutedEventArgs e)
        {
            string path = ((sender as Button).DataContext as SolutionModel).FolderPath;
            Process.Start("explorer.exe", path);
        }

        private void btnOpenInVS_Click(object sender, RoutedEventArgs e)
        {
            var selSol = ((sender as Button).DataContext as SolutionModel);
            selSol.TryBuildCode(AllSettings.CompilerDelay, AllSettings.DevVsPath);
            if (selSol.CompileErrors != "")
            {
                MessageBox.Show(selSol.CompileErrors.ToString());

            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbSearch.Text != "" || !string.IsNullOrEmpty(txbSearch.Text))
            {
                if (chkInCodeOnlySearch.IsChecked == false)
                    folderList.ItemsSource = allFiles.Where(p => p.Info.SorteerNaam.Contains(txbSearch.Text) || p.Path.Contains(txbSearch.Text)).OrderBy(p => p.Info.SorteerNaam);
                else
                {
                    if (txbSearch.Text.Length > 4)
                        folderList.ItemsSource = allFiles.Where(p => p.Code.Contains(txbSearch.Text)).OrderBy(p => p.Info.SorteerNaam);
                }
            }
            else folderList.ItemsSource = allFiles;
        }
        public void UpdateProgressText(double percentage)
        {
            progressBar.Value = percentage;

        }
        private async void testAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Dit kan lang duren.Ben je zeker?", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                ListBoxControls.IsEnabled = false;
                folderList.IsEnabled = false;
                progressBar.Visibility = Visibility.Visible;
                IProgress<double> progress = new Progress<double>(UpdateProgressText);

                try
                {
                    await Task.Run(() =>
                    {
                        double teller = 0;
                        int total = folderList.Items.Count;
                        progress.Report(0.01);
                        foreach (SolutionModel item in folderList.Items)
                        {
                            item.TryBuildCode(AllSettings.CompilerDelay, AllSettings.DevVsPath, false);
                            Debug.WriteLine($"Compiletest {item}:{item.CompileErrors}");
                            teller++;
                            progress.Report(teller / total);
                        }
                    }
                        );

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    folderList.IsEnabled = true;
                    ListBoxControls.IsEnabled = true;
                    progressBar.Visibility = Visibility.Collapsed;
                }

            }
        }
    }
}
