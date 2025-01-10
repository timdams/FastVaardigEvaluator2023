using FastSLNEvaluator2024.EvaluationLib;
using FastSLNEvaluator2024.EvaluationLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace FastSLNEvaluator2024.Windows
{
    /// <summary>
    /// Interaction logic for OplossingsWindow.xaml
    /// </summary>
    public partial class OplossingsWindow : Window
    {
        public OplossingsWindow()
        {
            InitializeComponent();
            lbExamen.DataContext = examenVM;
        }
        ExamenVM examenVM = new ExamenVM();
        private void Load_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText("examensample.json");
            Examen ingeladen = JsonSerializer.Deserialize<Examen>(json);
            examenVM.Load(ingeladen);
            
        }

        private void GeneratorJsonExampleFile()
        {
            Examen examen = new Examen();
            Opgave opgave1 = new Opgave();
            examen.Opgaves.Add(opgave1);

            Criteria crit1 = new Criteria()
            { Beschrijving = "criteria 1", MaxScore = 1 };
            Criteria crit2 = new Criteria()
            { Beschrijving = "criteria 2", MaxScore = 2 };
            opgave1.Criterias.Add(crit1);
            opgave1.Criterias.Add(crit2);

            Opgave opgave2 = new Opgave();
            examen.Opgaves.Add(opgave2);

            Criteria crit1a = new Criteria()
            { Beschrijving = "criteria 1a", MaxScore = 3 };
            Criteria crit2a = new Criteria()
            { Beschrijving = "criteria 2a", MaxScore = 4 };
            opgave2.Criterias.Add(crit1a);
            opgave2.Criterias.Add(crit2a);

            var examensj = JsonSerializer.Serialize(examen);
            
            File.WriteAllText("examensample.json", examensj);
            MessageBox.Show("Examensample.json gemaakt in " + Environment.CurrentDirectory);
            Process.Start("explorer.exe", Environment.CurrentDirectory);
        }

        private void genExampleJson_Click(object sender, RoutedEventArgs e)
        {
            GeneratorJsonExampleFile();
        }
    }
}
