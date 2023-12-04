using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace FastEvalCL
{
    public class SolutionModel
    {
        public string Code { get; private set; }
        public string ProjectPath
        {
            get
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
        public override string ToString()
        {
            return Info.ToString();
        }
        public string TryBuildCode(int compilerDelay, string devVsPath, bool alsoRun=true)
        {
            try
            {

                //// Set the compiler options.
                CSharpParseOptions parseOptions = new CSharpParseOptions(LanguageVersion.Latest);

                //// Parse the code into a SyntaxTree.
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Code, parseOptions);
                var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
                // Find the Main method
                var mainMethod = root.DescendantNodes()
                                     .OfType<MethodDeclarationSyntax>()
                                     .First(method => method.Identifier.ValueText == "Main");
                // Parse the new line of code
                var newLineSyntax = SyntaxFactory.ParseStatement("Console.ReadKey();\n");

                // Insert the new line into the Main method
                var newMainMethod = mainMethod.AddBodyStatements(newLineSyntax);
                root = root.ReplaceNode(mainMethod, newMainMethod);
                string res = BuildAndRunHelper.BuildAndRun(root.ToFullString(), compilerDelay, devVsPath, alsoRun);
                if (res == "")
                    this.Boete.CompileertNiet = Tested.Compileert;
                else this.Boete.CompileertNiet = Tested.CompileertNiet;
                return res;

            }
            catch (Exception)
            {
                try
                {
                    string res= BuildAndRunHelper.BuildAndRun(Code, compilerDelay, devVsPath,alsoRun);
                    if (res == "")
                        this.Boete.CompileertNiet = Tested.Compileert;
                    else this.Boete.CompileertNiet = Tested.CompileertNiet;
                    return res;
                }
                catch (Exception ex)
                {               
                    return ex.Message;
                }
            }

        }
    }
}
