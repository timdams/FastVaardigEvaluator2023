using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MakeLifeEasierWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            //TODO: werkt niet?
            dlg.SelectedPath = Properties.Settings.Default.LastPath;
       
            if (dlg.ShowDialog() == true)
            {
                Properties.Settings.Default.LastPath = dlg.SelectedPath;
                Properties.Settings.Default.Save();

                string[] allfiles = Directory.GetFiles(dlg.SelectedPath, "Program.cs", SearchOption.AllDirectories);
                folderList.ItemsSource = allfiles;
            }
        }

        private void folderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(folderList.SelectedItem!=null)
            {
                var res = FastEvalCL.FastEvalCL.GetInfoFromCode(File.ReadAllText(folderList.SelectedItem.ToString()));
                selInfo.DataContext = res;
                if(cmbCodeFilter.SelectedIndex==0)
                    textEditor.Text = File.ReadAllText(folderList.SelectedItem.ToString());
                else
                {
                    var regions = FastEvalCL.FastEvalCL.GetCodeRegions(File.ReadAllText(folderList.SelectedItem.ToString()));
                    if (regions.Count > cmbCodeFilter.SelectedIndex - 1)
                    {
                        textEditor.Text = regions[cmbCodeFilter.SelectedIndex - 1];
                    }
                    else
                        textEditor.Text = "********DEZE REGIO NIET GEVONDEN.HIERONDER ALLE CODE: ****\n\r" + File.ReadAllText(folderList.SelectedItem.ToString()); ;
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
            //i'm a hacker
            if(folderList.SelectedIndex>=0)
                folderList_SelectionChanged(this, null);
        }
    }
}
