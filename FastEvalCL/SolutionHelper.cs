using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FastEvalCL;


public class SolutionHelper
{
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
            return await workspace.OpenSolutionAsync(path);
        }
    }

    private static void CheckNeedRegister()
    {
        if (!RegistDefaultCalled)
        {
            MSBuildLocator.RegisterDefaults();
            RegistDefaultCalled = true;
        }
    }

    public static async Task<List<Solution>> LoadAllSolutionsFromPathAsync(string folderPath)
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





