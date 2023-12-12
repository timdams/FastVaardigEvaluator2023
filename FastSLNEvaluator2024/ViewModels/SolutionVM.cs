using CommunityToolkit.Mvvm.ComponentModel;
using FastEvalCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace FastSLNEvaluator2024.ViewModels
{

    public partial class SolutionVM : ObservableObject
    {
        [ObservableProperty]
        private string? path;

        public string ShortPath
        {
            get { return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path)); }
        }

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private ObservableCollection<ProjectVM> projects = new();

        [ObservableProperty]
        private ProjectVM selectedProject;

        [ObservableProperty]
        private StudentInfo studentInfo = new StudentInfo();

        public Visibility ProjectsVisibility
        {
            get
            {
                if (projects.Count > 1  )
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

       


        public Visibility IsDummyProjectsVisibility
        {
            get
            {
                if ( projects.Count==1 && projects.First().IsDummy)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        public SolutionVM(Microsoft.CodeAnalysis.Solution singeSln)
        {
            
            path = singeSln.FilePath;
            
            //TODOdummy info nnaar ui bre nnge
            projects.Clear();
            foreach (Microsoft.CodeAnalysis.Project proj in singeSln.Projects)
            {
                projects.Add(new ProjectVM(proj));
            }
            selectedProject = projects.FirstOrDefault();

            if (selectedProject != null)
                name = selectedProject.Name;
            else name = "NO PROJECT FOUND HERE:" + path;

            LoadAdditionalInfo();
        }

        internal void LoadAdditionalInfo()
        {
            var proj = projects.FirstOrDefault();
            if(proj != null)
            {
                var programcs= proj.Files.Where(p => p.FileName.ToLower() == "program.cs").FirstOrDefault();
                if (programcs != null)
                {
                    studentInfo = FastEvalCL.FastEvalCL.GetInfoFromCode(programcs.Code);
                    
                    OnPropertyChanged("StudentInfo");
                }
                
            }
        }
    }
}
