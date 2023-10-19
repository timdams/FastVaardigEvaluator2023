using FastEvalCL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ConsoleTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var code = @"#region ClassName.Test()    //Some method that does stuff
            //some stuff
            #endregion

                //VUL VOLGENDE INFORMATIE HIER IN
    //S-nummer              :  S00478
    //Voornaam              :  Jos   
    //Achternaam            :  Dams         
    //Klasgroep (bv 1IT1b)  :  1IT2b  

            #region ClassCName.Random()
            public static void Test()
            {
int meuh;
string Person;
            goto hell;

                 //Some more stuff

                goto 42;
            }
            #endregion";

            List<string> codeRegions = FastEvalCL.FastEvalCL.GetCodeRegions(code);

            foreach (var item in codeRegions)
            {
                Console.WriteLine("Region");
                Console.WriteLine(item);
            }

            var res= FastEvalCL.FastEvalCL.GetInfoFromCode(code);
            Console.WriteLine(  res);

            // var sum= FastEvalCL.FastEvalCL.BoeteControle(code);

            SolutionHelper.LoadSolution(@"C:\Temp\ConsoleApp1");

            //TODO https://learn.microsoft.com/en-us/visualstudio/msbuild/find-and-use-msbuild-versions?view=vs-2019

        }


    }
}
