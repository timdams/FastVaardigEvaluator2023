using FastEvalCL;
using Microsoft.CodeAnalysis;

namespace ConsoleTester
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //string slnPath= @"D:\Temp\__PPUNZ\Hafid Khorta_214945_assignsubmission_file_\PP2eZitHafidKhorta\";
            //string slnName = "PP2eZitHafidKhorta.sln";
            //    Task<Solution> sln =FastEvalCL.SolutionHelper.LoadSolutionFromPathAsync(slnPath+slnName);

            //var error = SolutionHelper.TestIfCompilesAsync(sln.Result.Projects.First());
            //if(error.Result !=null)
            //{
            //    foreach (var e in error.Result)
            //    {
            //        var err = e.ToString().Replace(slnPath,string.Empty);
            //        Console.WriteLine(err);

            //    }
            //}
            string slnPath = @"C:\temp\___unz";
            var slns = SolutionHelper.LoadAllSolutionsFromPathAsync(slnPath);
            foreach (var sl in slns.Result)
            {

                var error = SolutionHelper.TestIfCompilesAsync(sl.Projects.First());
                if (error.Result.Count() != 0)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("DID NOT COMPILE:" + sl.FilePath);

                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("DID COMPILE:" + sl.FilePath);
                    SolutionHelper.TryBuildAndRun(sl.Projects.First(), @"c:\temp\_run", sl.Projects.First().Name);
                }
                Console.ResetColor();
            }

        }



    }
}
