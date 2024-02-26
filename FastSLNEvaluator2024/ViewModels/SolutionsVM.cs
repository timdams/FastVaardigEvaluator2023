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

        private List<SolutionVM> allSolutions;
        public async Task LoadSolutionsAsync(string selectedPath)
        {

            var results = await SolutionHelper.LoadAllSolutionsFromPathAsync(selectedPath);
            solutions.Clear();

            allSolutions = new List<SolutionVM>(results.Select(singleSln => new SolutionVM(singleSln))
                            .OrderBy(p => p.StudentInfo.SorteerNaam));

            foreach (var item in allSolutions)
            {
                solutions.Add(item);
            }

        }

        public void FilterSolution(string textToSearch, bool alsoInFiles = false)
        {
            if (textToSearch != "" && alsoInFiles == false)
            {
                solutions.Clear();
                //TODO scherper maken
                List<SolutionVM> filter;

                filter = allSolutions
                .Where(
                         p => p.Name.ToLower().Contains(textToSearch.ToLower())
                        || p.StudentInfo.SorteerNaam.ToLower().Contains(textToSearch.ToLower())
                      )
                .OrderBy(p => p.StudentInfo.SorteerNaam).ToList();

                foreach (var item in filter)
                {
                    solutions.Add(item);
                }

            }
            else if (alsoInFiles == true && textToSearch.Length >= 3)
            {
                solutions.Clear();
                //TODO scherper maken
                List<SolutionVM> filter;

                filter = allSolutions
                .Where(
                         p => p.ContainsCode(textToSearch.ToLower())
                      )
                .OrderBy(p => p.StudentInfo.SorteerNaam).ToList();

                foreach (var item in filter)
                {
                    solutions.Add(item);
                }
            }
            else
            {
                solutions.Clear();
                foreach (var item in allSolutions)
                {
                    solutions.Add(item);
                }
            }
            OnPropertyChanged("Solutions");
        }

    }
}
