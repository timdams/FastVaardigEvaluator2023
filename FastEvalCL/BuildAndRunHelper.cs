using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL
{
    public class BuildAndRunHelper
    {

        public static string CompiledProgramName { get; set; } = @"temp\Program.exe";

        public static string BuildAndRun(string code, int compilerDelay, string devVsPath, bool alsoRun)
        {
            errors = "";
           string DevCmdPath = $"%comspec% /k \"{devVsPath}\""; 
            try
            {
                const string tempCodePath = "temp\\Program.cs";

                if (!Directory.Exists("temp"))
                {
                    Directory.CreateDirectory("temp");
                }

                if (File.Exists(tempCodePath))
                    File.Delete(tempCodePath);

                if (File.Exists(CompiledProgramName))
                    File.Delete(CompiledProgramName);

                //check for using System
                if (!code.Contains("using System;"))
                {
                    code = "using System;\r\n" + code;
                }
                File.WriteAllText(tempCodePath, code);

                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\temp";

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
                    process.StandardInput.WriteLine(DevCmdPath);

                    process.StandardInput.WriteLine($"csc.exe Program.cs");
                    System.Threading.Thread.Sleep(compilerDelay);
                    process.StandardInput.WriteLine("exit");

                    process.Kill();

                }

                if (File.Exists(CompiledProgramName) )
                {
                    if(alsoRun)
                        Process.Start(CompiledProgramName);
                    return "";
                }
                else
                    return "Compilatie niet gelukt. Fouten:\n" + errors ;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }

        private static string errors = "";

        public static void ProcessOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Debug.WriteLine(outLine.Data);
            if (outLine.Data!=null &&  outLine.Data.Contains("error"))
                errors += outLine.Data;
        }

        public static void ProcessErrorDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }
    }
}
