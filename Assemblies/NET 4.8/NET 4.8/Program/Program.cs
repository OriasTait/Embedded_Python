using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//=============
// Aliases
//=============
using Con = System.Console;

namespace NET_4_8
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

            // Hardcoded inline -c example
            Con.WriteLine("Running inline -c example:");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = "-c \"import sys; print(sys.version); "
                          + "print('Hello from embedded Python!')\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true, // kept for safety, but not printed
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process proc = Process.Start(psi))
            using (StreamReader reader = proc.StandardOutput)
            {
                string output = reader.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Run the hello.py script without parameters
            Con.WriteLine("Running script hello.py without arguments:");

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
                string[] quotedArguments = Array.ConvertAll(scriptArgs, arg => QuoteForCmd(arg));
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // not printed
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Run the hello.py script with parameters
            Con.WriteLine("Running script hello.py with arguments:");

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
                string[] quotedArguments = Array.ConvertAll(scriptArgs, arg => QuoteForCmd(arg));
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // not printed
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Run the add_numbers.py script with parameters
            Con.WriteLine("Running script add_numbers.py with arguments:");

            scriptRelativePath = pythonMyScripts + "\\add_numbers.py";
            scriptPath = Path.Combine(baseDir, scriptRelativePath);
            quotedScriptPath = QuoteForCmd(scriptPath);
            scriptArgs = new string[] { "5", "7" };

            if (scriptArgs == null || scriptArgs.Length == 0)
            {
                quotedArgs = string.Empty;
            }
            else
            {
                string[] quotedArguments = Array.ConvertAll(scriptArgs, arg => QuoteForCmd(arg));
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // not printed
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Run the make_message.py script with parameters
            Con.WriteLine("Running script add_numbers.py with arguments:");

            scriptRelativePath = pythonMyScripts + "\\make_message.py";
            scriptPath = Path.Combine(baseDir, scriptRelativePath);
            quotedScriptPath = QuoteForCmd(scriptPath);
            scriptArgs = new string[] { "Orias" };

            if (scriptArgs == null || scriptArgs.Length == 0)
            {
                quotedArgs = string.Empty;
            }
            else
            {
                string[] quotedArguments = Array.ConvertAll(scriptArgs, arg => QuoteForCmd(arg));
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // not printed
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Create a python script and run it
            Con.WriteLine("Running dynamically created script with arguments:");

            scriptRelativePath = pythonMyScripts + "\\Dynamic.py";
            scriptPath = Path.Combine(baseDir, scriptRelativePath);

            if (!File.Exists(scriptPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));
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
                string[] quotedArguments = Array.ConvertAll(scriptArgs, arg => QuoteForCmd(arg));
                quotedArgs = " " + string.Join(" ", quotedArguments);
            }
            arguments = quotedScriptPath + quotedArgs;

            psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // not printed
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process proc = Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);

                Console.WriteLine($"Python exit code: {proc.ExitCode}\n");
            }

            // Wait for the user to press a key before exiting
            Con.WriteLine("Press any key to exit...");
            Con.ReadKey();
        } // static void Main(/*string[] args*/)
    } // internal class Program
} // namespace NET_4_8
