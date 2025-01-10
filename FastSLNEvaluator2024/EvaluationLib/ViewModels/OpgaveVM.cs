using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FastSLNEvaluator2024.EvaluationLib.ViewModels
{
    internal partial class OpgaveVM: ObservableObject
    {
        private Opgave refToOrigOpgave = null;
        public OpgaveVM(Opgave opgave)
        {
            refToOrigOpgave = null;
            foreach (var criteria in opgave.Criterias)
            {
                criterias.Add(new CriteriaVM(criteria));
            }
            beschrijving = opgave.Beschrijving;
            maxScore = opgave.MaxScore;
        }

   

        [ObservableProperty]
        private ObservableCollection<CriteriaVM> criterias = new();

        [ObservableProperty]
        private int maxScore;


        [ObservableProperty]
        private string beschrijving;
    }

  
}