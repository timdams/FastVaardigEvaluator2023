using CommunityToolkit.Mvvm.ComponentModel;
using ICSharpCode.AvalonEdit;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FastSLNEvaluator2024.ViewModels
{
    using ICSharpCode.AvalonEdit.Document;
    using System.Diagnostics;

    public partial class FileVM : ObservableObject

    {
        private Document f;

        public string FileName { get => System.IO.Path.GetFileName(f.FilePath); }

        [ObservableProperty]
        private string code; 

        [ObservableProperty]
        private string fullPath;

        private TextDocument codeDocument;
        public TextDocument CodeDocument
        {
            get { return codeDocument; }
            set
            {
                if (codeDocument != value)
                {
                    codeDocument = value;
                    OnPropertyChanged();
                }
            }
        }

        public FileVM(Document f)
        {
            this.f = f;

            fullPath = f.FilePath;
            
            codeDocument = new TextDocument();
            
        }

        internal void LoadCode()
        {
            Debug.WriteLine($"Code loaded:{fullPath}");
            Code = System.IO.File.ReadAllText(fullPath);
            codeDocument.Text = Code;
            OnPropertyChanged("CodeDocument");
        }
    
    }
}
