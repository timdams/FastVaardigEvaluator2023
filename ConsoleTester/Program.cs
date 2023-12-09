using FastEvalCL;

using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Reflection;

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
            string slnPath = @"D:\Temp\__TEST";
            var slns = SolutionHelper.LoadAllSolutionsFromPathAsync(slnPath);
            foreach (var sl in slns.Result) 
            {
                var error = SolutionHelper.TestIfCompilesAsync(sl.Projects.First());
                if (error.Result.Count() != 0)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("DID NOT COMPILE:"+sl.FilePath);

                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine( "DID COMPILE:"+ sl.FilePath);
                }
                Console.ResetColor();
            }

        }



    }
}
