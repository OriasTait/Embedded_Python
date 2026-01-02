using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedded_Python
{
    internal static partial class Program
    {
        static void Main(/*string[] args*/)
        {
            // Initialize Python environment
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string pythonHome = Path.Combine(baseDir, "py_e");
            string pythonMyScripts = Path.Combine(baseDir, "py_scripts");
            string pythonLib = Path.Combine(pythonHome, "Lib");
            string pythonExe = Path.Combine(pythonHome, "python.exe");
            Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
            Environment.SetEnvironmentVariable("PYTHONPATH", pythonLib);

            // hardcoded example of running a python script
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = "-c \"import sys; print(sys.version); print('Hello from embedded Python!')\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Replace using declarations with using statements for C# 7.3 compatibility
            using (Process proc = Process.Start(psi))
            using (StreamReader reader = proc.StandardOutput)
            {
                string output = reader.ReadToEnd();
                Console.WriteLine(output);
            }

            // Run a second example with a script file
            string scriptRelativePath = pythonMyScripts + "\\hello.py";
            string scriptPath = Path.Combine(baseDir, scriptRelativePath);
            string quotedScriptPath = QuoteForCmd(scriptPath);
            string[] scriptArgs = null;

            string quotedArgs = string.Empty;
            string arguments = string.Empty;

            if (scriptArgs == null || scriptArgs.Length == 0)
            {
                quotedArgs = string.Empty;
            }
            else
            {
                // Quote each argument so cmd.exe handles spaces correctly
                string[] quotedArguments = Array.ConvertAll(
                    scriptArgs,
                    arg => QuoteForCmd(arg)
                );

                // Join arguments with spaces and add a leading space
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            // Reuse ProcessStartInfo to run the script
            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Read both streams and wait for exit to avoid deadlocks
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                Console.WriteLine(output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.Error.WriteLine(error);
                }

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Run the second example script file with parameters
            scriptRelativePath = pythonMyScripts + "\\hello.py";
            scriptPath = Path.Combine(baseDir, scriptRelativePath);
            quotedScriptPath = QuoteForCmd(scriptPath);
            scriptArgs = new string[] { "arg1", "arg 2" };

            if (scriptArgs == null || scriptArgs.Length == 0)
            {
                quotedArgs = string.Empty;
            }
            else
            {
                // Quote each argument so cmd.exe handles spaces correctly
                string[] quotedArguments = Array.ConvertAll(
                    scriptArgs,
                    arg => QuoteForCmd(arg)
                );

                // Join arguments with spaces and add a leading space
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            // Reuse ProcessStartInfo to run the script
            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Read both streams and wait for exit to avoid deadlocks
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                Console.WriteLine(output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.Error.WriteLine(error);
                }

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Create a python script and run it
            scriptRelativePath = pythonMyScripts + "\\Dynamic.py";
            scriptPath = Path.Combine(baseDir, scriptRelativePath);

            // Create the directory if it doesn't exist
            if (!File.Exists(scriptPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(scriptPath) );
            }

            File.WriteAllText(scriptPath,
@"import sys
print(sys.executable)
print(sys.version)
print('Hello from external Dynamic Python script file!')
print('Received args:', sys.argv[1:])");

            quotedScriptPath = QuoteForCmd(scriptPath);
            scriptArgs = new string[] { "arg3", "arg 4" };

            if (scriptArgs == null || scriptArgs.Length == 0)
            {
                quotedArgs = string.Empty;
            }
            else
            {
                // Quote each argument so cmd.exe handles spaces correctly
                string[] quotedArguments = Array.ConvertAll(
                    scriptArgs,
                    arg => QuoteForCmd(arg)
                );

                // Join arguments with spaces and add a leading space
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            // Reuse ProcessStartInfo to run the script
            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Read both streams and wait for exit to avoid deadlocks
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                Console.WriteLine(output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.Error.WriteLine(error);
                }

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Wait for the user to press a key before exiting
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        } // static void Main(/*string[] args*/)
    } // internal class Program
} // Embedded_Python
