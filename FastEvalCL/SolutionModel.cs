using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Diagnostics;
using System.Text.Json;

namespace FastEvalCL
{
    public class SolutionModel
    {
        public string Code { get; private set; }
        public string ProjectPath { get
            {
                return "C:\\temp\\ConsoleApp3\\ConsoleApp3\\ConsoleApp3.csproj";
            } 
        }

        public SolutionModel(string path, bool tryFetchInfo = true)
        {
            Path = path;

            //TODO kijken of boete file in map staat en inladen
            string boeteFile = System.IO.Path.Combine(FolderPath, "boete.json");
            if (File.Exists(boeteFile))
            {
                string jsonString = File.ReadAllText(boeteFile);
                Boete = JsonSerializer.Deserialize<Boete>(jsonString);
            }
            else
                Boete = new Boete();
            if (tryFetchInfo)
            {
                Code = File.ReadAllText(path);
                Info = FastEvalCL.GetInfoFromCode(Code);
            }
            else
                Info = new Info()
                {
                    AchterNaam = "Onbekend",
                    VoorNaam = "Onbekend",
                    Klasgroep = "Onbekend",
                    SNummer = path
                };
        }
        public string Path { get; set; }

        public string FolderPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path);
            }
        }

        public Info Info { get; set; }
        public Boete Boete { get; set; }

        public void SafeBoete()
        {
            if (Boete != null)
            {
                string fileName = System.IO.Path.Combine(FolderPath, "boete.json");
                string jsonString = JsonSerializer.Serialize(Boete);
                File.WriteAllText(fileName, jsonString);
            }
        }

        public string TryBuildCode()
        {
            // Set the compiler options.
            CSharpParseOptions parseOptions = new CSharpParseOptions(LanguageVersion.Latest);

            // Parse the code into a SyntaxTree.
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Code, parseOptions);

            // Set the compiler options for the C# compiler.
            CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // Create a compilation with a reference to mscorlib (for system-related types).
            var compilation = CSharpCompilation.Create("MyProgram")
                .WithOptions(compilationOptions)
                 .AddReferences(
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // Add reference to System.Core
                MetadataReference.CreateFromFile(typeof(Activator).Assembly.Location)) // Add reference to System.Runtime
                .AddSyntaxTrees(syntaxTree);

            // Check for compilation errors.
            var diagnostics = compilation.GetDiagnostics();

            if (!diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
            {
                
                return "Compilation successful.";
            }
            else
            {
                string res= "Compilation failed. Errors:";
                foreach (var diagnostic in diagnostics)
                {
                    if (diagnostic.Severity == DiagnosticSeverity.Error)
                    {
                        res+=(diagnostic.ToString())+ "\n";
                    }
                }
                return res;
            }
            
        }
    }
}
