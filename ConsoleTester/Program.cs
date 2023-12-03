using FastEvalCL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text.Json;

namespace ConsoleTester
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string code = @"using System;
namespace pp_tweedezit_Hayati_Sahin
{
    internal class Program
    {
        static void Main(string[] args)
        {
          Console.WriteLine(""Hallo2"");

        }

    }
    
}";
            BuildAndRunHelper.BuildAndRun(code);

        }



    }
}
