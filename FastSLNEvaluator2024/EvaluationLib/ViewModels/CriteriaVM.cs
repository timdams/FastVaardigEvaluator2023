using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastSLNEvaluator2024.EvaluationLib.ViewModels
{
    internal partial class CriteriaVM: ObservableObject
    {
        private Criteria refToOrigCriteria;

        public CriteriaVM(Criteria criteria)
        {
            refToOrigCriteria = criteria;
            beschrijving = criteria.Beschrijving;
            maxScore = criteria.MaxScore;
        }

        [ObservableProperty]
        private string beschrijving;

        [ObservableProperty]
        private int maxScore;

    }
}
