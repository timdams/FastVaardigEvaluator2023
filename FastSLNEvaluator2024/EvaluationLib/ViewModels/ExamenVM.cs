using CommunityToolkit.Mvvm.ComponentModel;
using FastSLNEvaluator2024.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastSLNEvaluator2024.EvaluationLib.ViewModels
{
    internal partial class ExamenVM : ObservableObject
    {
        private Examen refToOrigExamen = null;
        public void Load(Examen examenInteladen)
        {
            refToOrigExamen = examenInteladen;
            foreach (var opgave in examenInteladen.Opgaves)
            {
                opgaves.Add(new OpgaveVM(opgave));
            }
            maxScore = examenInteladen.MaxScore;
        }

        [ObservableProperty]
        private ObservableCollection<OpgaveVM> opgaves = new();

        [ObservableProperty]
        private int maxScore;
    }
}
