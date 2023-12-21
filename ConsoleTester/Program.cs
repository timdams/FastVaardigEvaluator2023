using FastEvalCL;
using Microsoft.CodeAnalysis;

namespace ConsoleTester
{
    internal class Program
    {
        static void Main(string[] args)
        {

            MoodleOpdrachtUnpacker.UnpackAllOpdrachten(@"d:\Temp\_SZIP\tryme.zip", @"d:\Temp\_SZIP\");
        }



    }
}
