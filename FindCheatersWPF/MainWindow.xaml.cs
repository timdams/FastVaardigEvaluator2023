using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace FindCheatersWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO java api van deze aanroepen? https://github.com/jplag/JPlag?tab=readme-ov-file
        public MainWindow()
        {
            InitializeComponent();
        }
        ObservableCollection<Code> codes = new ObservableCollection<Code>();
        private void Load_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            DirectoryInfo directory = new DirectoryInfo(@"E:\Download\OOP24");
           var files= directory.GetFiles("pharaoh.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string[] lines = File.ReadAllLines(file.FullName);
                
                codes.Add(new Code { FCode = string.Join("\n", lines), Path = file.FullName });
            }
        }

        private void lbCode_Loaded(object sender, RoutedEventArgs e)
        {
            lbCode.ItemsSource = codes;
        }
    }
    class Code
    {
        public string FCode { get; set; }
        public string Path { get; set; }
    }
}