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

            var sortedIncoming = results.Select(singleSln => new SolutionVM(singleSln))
                            .OrderBy(p => p.StudentInfo.SorteerNaam);

            foreach (var item in sortedIncoming)
            {
                solutions.Add(item);
            }

        }


    }
}
