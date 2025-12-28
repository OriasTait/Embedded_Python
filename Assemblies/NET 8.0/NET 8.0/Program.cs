using System.Diagnostics;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Embedded_Python
#pragma warning restore IDE0130 // Namespace does not match folder structure
/*=============
using classic style namespace declaration because .NET 8 duplicates the namespace calls
without it.  Example to call main method:
- With:     Embedded_IPython.Program.Main()
- Without:  Embedded_IPython.Embedded_IPython.Program.Main()
=============*/
{
    internal static class Program
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
            using (Process proc = Process.Start(psi)!)
            using (StreamReader reader = proc.StandardOutput)
            {
                string output = reader.ReadToEnd();
                Console.WriteLine(output);
            }

            // Run a second example with a script file
            string scriptRelativePath = pythonMyScripts + "\\hello.py";
            string scriptPath = Path.Combine(baseDir, scriptRelativePath);
            string quotedScriptPath = QuoteForCmd(scriptPath);
            string[]? scriptArgs = null;

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
            using (Process proc = Process.Start(psi)!)
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
            using (Process proc = Process.Start(psi)!)
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
                var dirName = Path.GetDirectoryName(scriptPath);
                if (!string.IsNullOrEmpty(dirName)) // Fix: ensure dirName is not null
                {
                    Directory.CreateDirectory(dirName); // No dereference of null
                }
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
            using (Process proc = Process.Start(psi)!)
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
        } // static void Main(string[] args)

        static string QuoteForCmd(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "\"\"";

            return "\"" + s.Replace("\"", "\\\"") + "\"";
        }
    } // internal static class Program
} // namespace Embedded_Python