using Microsoft.Build.Evaluation;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using System.Diagnostics;


namespace FastEvalCL;


public class SolutionHelper
{

    static string dummyPath = "data\\DummyProject\\DummySLN.sln";

    private static bool RegistDefaultCalled = false;
    public static async Task<Solution> LoadSolutionFromPathAsync(string path)
    {
        CheckNeedRegister();

        using (var workspace = MSBuildWorkspace.Create())
        {
#if DEBUG
            workspace.WorkspaceFailed += Workspace_WorkspaceFailed;
#endif
            Solution sln = null;
            try
            {
                sln = await workspace.OpenSolutionAsync(path);
            }
            catch (Exception e)
            {

                //TODO create dummy project that contains relevant information
                Debug.WriteLine("Creating dummy for:" + path);
                var slnDum = await workspace.OpenSolutionAsync(dummyPath);
                return slnDum.WithProjectName(slnDum.Projects.First().Id, "DUMMY__" + Path.GetFileName(path));

            }

            return sln;
        }
    }

    private static void Workspace_WorkspaceFailed(object? sender, WorkspaceDiagnosticEventArgs e)
    {
        Debug.WriteLine(e.Diagnostic.ToString());
    }

    private static void CheckNeedRegister()
    {
        if (!RegistDefaultCalled)
        {
            MSBuildLocator.RegisterDefaults();
            var q = MSBuildLocator.QueryVisualStudioInstances();
            //Volgende lijn is ESSENTIEEL! Anders zal WPF huilen en geen projecten inladen (enkel slns)
            //https://stackoverflow.com/questions/38204509/roslyn-throws-the-language-c-is-not-supported

            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            RegistDefaultCalled = true;
        }
    }

    public static async Task<IEnumerable<Solution>> LoadAllSolutionsFromPathAsync(string folderPath)
    {
        var result = new List<Solution>();
        var allPrograms = Directory.GetFiles(folderPath, "*.sln", SearchOption.AllDirectories);
        foreach (var prog in allPrograms)
        {
            if (!prog.Contains("__MACOSX")) //soort shadow/git map op macosx waar onbruikbare sln in zit...danku apple
            {
                var sl = await LoadSolutionFromPathAsync(prog);
                result.Add(sl);
            }
            //TODO fetch info hier?? ni echt goeie oop

        }
        return result;
    }

    public static async Task<IEnumerable<Diagnostic>> TestIfCompilesAsync(Microsoft.CodeAnalysis.Project project)
    {
        var compilation = project.GetCompilationAsync().Result;
        var errors = compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

        return errors;
    }



    public enum DotNetVersions { NET8, NET7, NET6, NET5 };
    //TODO versie vragen via settings in UI
    public static async void TryBuildAndRun(Microsoft.CodeAnalysis.Project project, string outputPath, string runtimeFileNameWithExt, DotNetVersions dotNetVersion = DotNetVersions.NET6)
    {

        var compilation = project.GetCompilationAsync().Result;
        var errors = compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        if (errors.Count() == 0)
        {
            //En nu compileren maar...kaarsjes branden helpt hier zeker.
            string outputFilePath = System.IO.Path.Combine(outputPath, runtimeFileNameWithExt + ".dll");
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);
            var res = compilation.Emit(outputFilePath);
            if (res.Success)
            {
                //Nu bijhorende configfile maken zodat dotnet myfile.dll werkt later
                string configText = GenerateJSONConfigText(dotNetVersion);
                string configPath = System.IO.Path.Combine(outputPath, runtimeFileNameWithExt + ".runtimeconfig.json");
                if (File.Exists(configPath))
                    File.Delete(configPath);
                File.WriteAllText(configPath, configText);

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "dotnet";
                psi.Arguments = outputFilePath;
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();
            }
        }

    }
    private static string GenerateJSONConfigText(DotNetVersions version)
    {
        //meer info: https://learn.microsoft.com/en-us/dotnet/standard/frameworks

        string tfm = "";
        string versionText = "";
        switch (version)
        {
            case DotNetVersions.NET8:
                tfm = "net8.0";
                versionText = "8.0.0";
                break;
            case DotNetVersions.NET7:
                tfm = "net7.0";
                versionText = "7.0.0";
                break;
            case DotNetVersions.NET6:
                tfm = "net6.0";
                versionText = "6.0.0";
                break;
            case DotNetVersions.NET5:
                tfm = "net5.0";
                versionText = "5.0.0";
                break;
            default:
                break;
        }

        string configText = @"
                {
                  ""runtimeOptions"": {
                    ""tfm"": ""TFMV"",
                    ""framework"": {
                      ""name"": ""Microsoft.NETCore.App"",
                      ""version"": ""VERV""
                    }
                  }
                }";

        return configText.Replace("TFMV", tfm).Replace("VERV", versionText);
    }
}





