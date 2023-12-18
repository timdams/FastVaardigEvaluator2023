﻿using CommunityToolkit.Mvvm.ComponentModel;
using FastEvalCL;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

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
                if (projects.Count > 1)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }


        public Visibility IsDummyProjectsVisibility
        {
            get
            {
                if (projects.Count == 1 && projects.First().IsDummy)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        public SolutionVM(Microsoft.CodeAnalysis.Solution singeSln)
        {

            path = singeSln.FilePath;

            //TODO dummy info naar ui brengen
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
            //Read comment tags from first project
            var proj = projects.FirstOrDefault();
            if (proj != null)
            {
                var programcs = proj.Files.Where(p => p.FileName.ToLower() == "program.cs").FirstOrDefault();
                if (programcs != null)
                {
                    studentInfo = FastEvalCL.FastEvalCL.GetInfoFromCode(programcs.Code);

                    OnPropertyChanged("StudentInfo");
                }

            }
        }
        [ObservableProperty]
        private bool canRun = true;

        [ObservableProperty]
        private Visibility visibilityCompileError = Visibility.Collapsed;

        [ObservableProperty]
        private string compileErrors = "";

        public void TestIfCompiles(bool alsoRun = false)
        {
            if (selectedProject != null)
            {
                var res = SolutionHelper.TestIfCompilesAsync(selectedProject.MSBuildProject);
                if (res.Result.Count() == 0)
                {
                    visibilityCompileError = Visibility.Collapsed;

                    if (alsoRun)
                    {
                        //TODO dotnet versie uit settings halen
                        SolutionHelper.TryBuildAndRun(selectedProject.MSBuildProject, "tempRun", selectedProject.Name, SolutionHelper.DotNetVersions.NET6);
                    }
                }
                else
                {
                    visibilityCompileError = Visibility.Visible;
                    string errorText = "";
                    foreach (var line in res.Result)
                    {
                        errorText += line.ToString(); //TODO meer propere output
                    }
                    compileErrors = errorText;
                    canRun = false;

                }
                OnPropertyChanged("CompileErrors");
                OnPropertyChanged("CanRun");
                OnPropertyChanged("VisibilityCompileError");
                //TODO compile errors naar UI brengen 
            }
        }

        internal void TryRun()
        {
            TestIfCompiles(true);
        }

        internal void OpenInExplorer()
        {
            Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(path));
        }

        internal void OpenInVS()
        {
            Process.Start("explorer.exe", path);
        }
    }
}
