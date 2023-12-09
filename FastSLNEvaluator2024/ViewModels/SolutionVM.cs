using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastSLNEvaluator2024.ViewModels
{

    public partial class SolutionVM : ObservableObject
    {
        [ObservableProperty]
        private string? path;

        [ObservableProperty]
        private ObservableCollection<ProjectVM> projects = new ();

        [ObservableProperty]
        private ProjectVM selectedProject;
        public SolutionVM(Microsoft.CodeAnalysis.Solution singeSln)
        {
            path = singeSln.FilePath;
            projects.Clear();
            foreach (Microsoft.CodeAnalysis.Project proj in singeSln.Projects)
            {
                projects.Add(new ProjectVM(proj));
            }
            selectedProject = projects.FirstOrDefault();

           
        }
    }
}
