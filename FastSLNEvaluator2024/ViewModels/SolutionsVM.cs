

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FastEvalCL;
using System.Collections.ObjectModel;

namespace FastSLNEvaluator2024.ViewModels
{
    public partial class SolutionsVM:ObservableObject
    {
        public SolutionsVM()
        {
            LoadSolutionsCommand = new AsyncRelayCommand(LoadSolutionsAsync);
        }

        public IAsyncRelayCommand LoadSolutionsCommand { get;  }

        [ObservableProperty]
        private ObservableCollection<SolutionVM> solutions  = new ();

        [ObservableProperty]
        private SolutionVM selectedSolution;

        

        private async Task LoadSolutionsAsync()
        {
            
            var results = await SolutionHelper.LoadAllSolutionsFromPathAsync(@"D:\Temp\__TEST");
            solutions.Clear();
            foreach (var singleSln in results)
            {
                solutions.Add(new SolutionVM(singleSln)); 
            }

        }

    }
}
