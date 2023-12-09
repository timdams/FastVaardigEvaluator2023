using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FastSLNEvaluator2024.ViewModels
{
    public partial class ProjectVM : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<FileVM> files = new();


        private FileVM selectedFile;

        public FileVM SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                selectedFile.LoadCode();
                OnPropertyChanged();
            }
        }

        public ProjectVM(Microsoft.CodeAnalysis.Project project)
        {
            name = project.Name;
            files.Clear();
            foreach (Microsoft.CodeAnalysis.Document f in project.Documents)
            {
                if (!f.FilePath.Contains(@"\obj\Debug\")) //TODO: elders definieren (settings?)
                    files.Add(new FileVM(f));


            }
            SelectedFile = files.Where(p => p.FileName.ToLower() == "program.cs").FirstOrDefault();
            if (selectedFile == null)
            {
                SelectedFile = files.FirstOrDefault();
            }
        }
    }
}
