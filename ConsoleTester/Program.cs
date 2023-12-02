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
            const string startPath = @"D:\Temp\__PPUNZ\Hafid Khorta_214945_assignsubmission_file_\PP2eZitHafidKhorta\PP2eZitHafidKhorta";
            const string codeFileName = "Program.cs";
            const string compiledProgramName = "Program.exe";
            const string devCmdPath= "%comspec% /k \"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\Tools\\VsDevCmd.bat\"";
            const int compileDelay = 2000; //make larger on slow computers

            string fullProgramPath = Path.Combine(startPath, compiledProgramName);
            string fullCodePath = Path.Combine(startPath, codeFileName);

            if (File.Exists(fullProgramPath))
                File.Delete(fullProgramPath);

            //check for using System
            var code = File.ReadAllText(fullCodePath);
            if (!code.Contains("using System;"))
            {
                var newCode = "using System;\r\n" + code;
                File.WriteAllText(fullCodePath, newCode);
            }

            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WorkingDirectory = startPath;
                
                process.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");

                // Redirects the standard input so that commands can be sent to the shell.
                process.StartInfo.RedirectStandardInput = true;
                // Runs the specified command and exits the shell immediately.
                //process.StartInfo.Arguments = @"/c ""dir""";

                process.OutputDataReceived += ProcessOutputDataHandler;
                process.ErrorDataReceived += ProcessErrorDataHandler;
                process.EnableRaisingEvents = true;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Send a directory command and an exit command to the shell
                process.StandardInput.WriteLine(devCmdPath);

                process.StandardInput.WriteLine($"csc.exe {codeFileName}");
                System.Threading.Thread.Sleep(compileDelay);
                process.StandardInput.WriteLine("exit");


                process.Kill();



            }
            //  Console.Clear();
            if (File.Exists(fullProgramPath))
            {
                Console.Clear();
                Process.Start(fullProgramPath); 
            }
            else
                Console.WriteLine("Kon niet gecompileerd worden");
        }

        public static void ProcessOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }

        public static void ProcessErrorDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }


    }
}
