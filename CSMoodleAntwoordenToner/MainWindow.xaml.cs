using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace CSMoodleAntwoordenToner
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

        private void Load_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var file = System.IO.File.ReadAllText(dlg.FileName);
                studRecords.Clear();
                ParseFile(file);
                lbAntwoorden.Items.Add("Antwoord 1");
                lbAntwoorden.Items.Add("Antwoord 2");
                lbAntwoorden.Items.Add("Antwoord 3");
            }
        }
        List<StudentRecord> studRecords = new List<StudentRecord>();
        private void ParseFile(string file)
        {
            
            using (var reader = new StringReader(file))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            }))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                foreach (IDictionary<string, object> item in records)
                {
                    // if (item is IDictionary<string, object> dictionary && dictionary.ContainsKey("Achternaam"))
                    {
                        var studRed = new StudentRecord()
                        {
                            Achternaam = item["Achternaam"].ToString(),
                            Voornaam = item["Voornaam"].ToString(),
                            EmailAdres = item["E-mailadres"].ToString(),
                            Status = item["Status"].ToString(),
                            GestartOp = item["Gestart op"].ToString(),
                            VoltooidOp = item["Voltooid"].ToString(),
                            Antwoord1 = item["Antwoord 1"].ToString(),
                            Antwoord2 = item["Antwoord 2"].ToString(),
                            Antwoord3 = item["Antwoord 3"].ToString(),
                        };
                        studRecords.Add(studRed);
                    }
                }
            }
            
            lbSollist.ItemsSource = studRecords;

        }

        private void lbSollist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbSollist.SelectedIndex>-1)
            {
                if(lbAntwoorden.SelectedIndex<0)
                {
                    lbAntwoorden.SelectedIndex = 0;
                }

                switch(lbAntwoorden.SelectedIndex)
                {
                    case 0:

                        textEditor.Text = studRecords[lbSollist.SelectedIndex].Antwoord1Clean;
                        break;
                    case 1:
                        textEditor.Text = studRecords[lbSollist.SelectedIndex].Antwoord2Clean;
                        break;
                    case 2:
                        textEditor.Text = studRecords[lbSollist.SelectedIndex].Antwoord3Clean;
                        break;
                }

        
            }
            
        }
    }
}