using FastEvalCL;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

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
                IOrderedEnumerable<SolutionModel> allfiles = SolutionHelper.LoadAllSolutionsFromPath(dlg.SelectedPath).OrderBy(p => p.Info.SorteerNaam);
                folderList.ItemsSource = allfiles;
            }
        }

        private void folderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (folderList.SelectedItem != null)
            {
                var activeSolVM = (folderList.SelectedItem as SolutionModel);
                selInfo.DataContext = activeSolVM.Info;
                boeteHeader.DataContext = activeSolVM.Boete;
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

            //TODO  settings wegschrijven/inladen
            ReloadUI();

        }

        private void ReloadUI()
        {
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

        private void btnLaadTemplate_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dlg.Filter = "Toetstemplates|*.json";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    string jsonString = File.ReadAllText(dlg.FileName);
                    huidigeTemplate = JsonSerializer.Deserialize<ToetsTemplate>(jsonString);
                    GenerateVerbeterControls();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Helaas, deze json file kon ik niet verwerken. Sorry. Meer info: " + ex.Message);
                }
            }

        }

        private void GenerateVerbeterControls()
        {
            //per categorie een tab
            if (huidigeTemplate != null)
            {
                var cats = huidigeTemplate.Vragen.GroupBy(p => p.Categorie);
                foreach (var cat in cats)
                {
                    TabItem tb = new TabItem() { Header = cat.Key };
                    WrapPanel wp = new WrapPanel() { Orientation = Orientation.Vertical };
                    tb.Content = wp;
                    foreach (var vraag in cat)
                    {
                        Control toAdd = null; ;
                        if (vraag.MaxScore == 1)
                        {
                            toAdd = new CheckBox() { Content = vraag.Beschrijving };
                            //TODO binding
                            wp.Children.Add(toAdd);
                        }
                        else if (vraag.MaxScore == 0)
                        {
                            StackPanel sp = new StackPanel();
                            sp.Children.Add(new TextBlock() { Text = vraag.Beschrijving });
                            sp.Children.Add(new TextBox());
                            //TODO binding
                            wp.Children.Add(sp);
                        }
                        else
                        {
                            StackPanel sp = new StackPanel();
                            sp.Children.Add(new TextBlock() { Text = vraag.Beschrijving });
                            Slider slider = new Slider()
                            {
                                MinWidth = 50,
                                Minimum = 0,
                                Maximum = vraag.MaxScore,
                                Interval = 1,
                                SmallChange= 1,
                                TickPlacement= System.Windows.Controls.Primitives.TickPlacement.BottomRight,
                                IsSnapToTickEnabled= true,
                                TickFrequency=1                        
                            };
                            
                            sp.Children.Add(slider);
                            //TODO binding
                            wp.Children.Add(sp);
                        }
            

                    }
                    tabControl.Items.Add(tb);
                }
            }
        }
    }
}
