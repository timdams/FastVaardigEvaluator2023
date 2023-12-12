using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FastEvalCL;


public class SolutionHelper
{

    static string dummyPath = "D:\\Dropbox\\PROGPROJECTS\\FastVaardigEvaluator2023\\DUMMYSLN\\DummySLN\\DummySLN.sln";//TODO in settings?
    public static List<SolutionModel> LoadAllSolutionsFromPath(string folderPath, bool tryFetchInfo = true)
    {
        List<SolutionModel> result = new List<SolutionModel>();
        var allPrograms = Directory.GetFiles(folderPath, "Program.cs", SearchOption.AllDirectories);
        foreach (var prog in allPrograms)
        {
            result.Add(new SolutionModel(prog, tryFetchInfo));
        }
        return result;
    }

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

                //TODO: create dummy project that contains relevant information
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
            var sl = await LoadSolutionFromPathAsync(prog);
            result.Add(sl);
            //TODO fetch info hier?? ni echt goeie oop

        }
        return result;
    }

    public static async Task<IEnumerable<Diagnostic>> TestIfCompilesAsync(Project project)
    {
        var compilation = project.GetCompilationAsync().Result;
        var errors = compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
        return errors;
    }
}





