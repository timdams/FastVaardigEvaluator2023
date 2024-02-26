using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FastSLNEvaluator2024.ViewModels
{
    public partial class ProjectVM : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<FileVM> files = new();

        [ObservableProperty]
        private bool isDummy = false;

        private FileVM selectedFile;

        public FileVM SelectedFile
        {
            get { return selectedFile; }
            set
            {
                if (value != null)
                {
                    selectedFile = value;
                    selectedFile.LoadCode();
                }

                OnPropertyChanged();
            }
        }
        public Microsoft.CodeAnalysis.Project MSBuildProject { get; private set; }
        public ProjectVM(Microsoft.CodeAnalysis.Project project)
        {
            MSBuildProject = project;
            name = project.Name;
            if (name.Contains("DUMMY__"))
                isDummy = true;

            files.Clear();
            foreach (Microsoft.CodeAnalysis.Document f in project.Documents)
            {
                if (!f.FilePath.Contains(@"\obj\Debug\")) //TODO elders definieren (settings?)
                    files.Add(new FileVM(f));


            }
            SelectedFile = files.Where(p => p.FileName.ToLower() == "program.cs").FirstOrDefault();
            if (SelectedFile == null)
            {
                SelectedFile = files.FirstOrDefault();
            }
        }

        internal bool ContainsCode(string textToSearch)
        {
            foreach (var file in Files)
            {
                if (file.ContainsCode(textToSearch))
                    return true;
            }
            return false;
        }
    }
}
