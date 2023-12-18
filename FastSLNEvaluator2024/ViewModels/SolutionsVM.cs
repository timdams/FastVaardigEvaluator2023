

using CommunityToolkit.Mvvm.ComponentModel;
using FastEvalCL;
using System.Collections.ObjectModel;

namespace FastSLNEvaluator2024.ViewModels
{
    public partial class SolutionsVM : ObservableObject
    {



        [ObservableProperty]
        private ObservableCollection<SolutionVM> solutions = new();

        [ObservableProperty]
        private SolutionVM selectedSolution;


        public async Task LoadSolutionsAsync(string selectedPath)
        {

            var results = await SolutionHelper.LoadAllSolutionsFromPathAsync(selectedPath);
            solutions.Clear();

            List<SolutionVM> incoming = new List<SolutionVM>();
            foreach (var singleSln in results)
            {
                incoming.Add(new SolutionVM(singleSln));
            }
            incoming = incoming.OrderBy(p => p.StudentInfo.SorteerNaam).ToList();

            foreach (var singleSln in incoming)
            {
                solutions.Add(singleSln);
            }
        }


    }
}
