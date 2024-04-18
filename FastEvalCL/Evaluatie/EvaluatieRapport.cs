using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL.Evaluatie
{
    public class EvaluatieRapport
    {
        public void LaadRapport(string path)
        {
            if(File.Exists(path))
            {
                //punten uitlezen en in collectie steken
            }
            else
            {
                //file aanmaken volgens huidige actieve rapport template 
            }
            Evaluaties.Add(new EvalItem() { Naam ="Test"});
            Evaluaties.Add(new EvalItem() { Naam = "Test" });
            Evaluaties.Add(new EvalItem() { Naam = "Test" });
        }
        public ObservableCollection<EvalItem> Evaluaties { get; set; } = new ObservableCollection<EvalItem>();
    }
}
