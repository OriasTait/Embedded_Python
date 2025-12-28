using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Embedded_IPython
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
            // Must be set before any libraries that may use BinaryFormatter are loaded
            AppContext.SetSwitch(
            "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization",
            true);

            string pythonEmbedPath = Path.Combine(AppContext.BaseDirectory, "py_e");
            string pythonDllName = "python313.dll";

            Environment.SetEnvironmentVariable("PYTHONHOME", pythonEmbedPath);

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string pythonPath = Path.Combine(baseDir, "Python");
            string pythonScriptsPath = Path.Combine(baseDir, "py_scripts");

            Environment.SetEnvironmentVariable("PYTHONPATH", pythonPath);

            Environment.SetEnvironmentVariable(
                "PYTHONNET_PYDLL",
                Path.Combine(pythonEmbedPath, pythonDllName));

            PythonEngine.Initialize();

            try
            {
                using (Py.GIL()) // Python Global Interpreter Lock
                {
                    dynamic sys = Py.Import("sys");
                    // folder containing IPython and dependencies
                    sys.path.append("embedded_py_libs");

                    // Configure IPython to run in embedded mode without spawning 
                    // new shells
                    dynamic os = Py.Import("os");
                    os.environ["PYDEVD_DISABLE_FILE_VALIDATION"] = "1".ToPython();

                    // Import IPython modules
                    dynamic interactiveshellModule =
                        Py.Import("IPython.core.interactiveshell");
                    dynamic InteractiveShell =
                        interactiveshellModule.GetAttr("InteractiveShell");

                    // Create shell instance with configuration to prevent subprocess 
                    // spawning
                    dynamic shell = InteractiveShell.InvokeMethod("instance");

                    // Store reference to properly dispose
                    PyObject systemDelegate = null;
                    try
                    {
                        systemDelegate = new Func<string, object>((cmd) =>
                        {
                            Console.WriteLine($"Blocked system call: {cmd}");
                            return string.Empty;
                        }).ToPython();
                        shell.system = systemDelegate;

                        string To_Run = string.Empty;

                        // Run a normal Python line
                        Console.WriteLine("Run a normal Python line:");
                        To_Run = $"print('Hello from IPython shell!')";
                        shell.run_cell(To_Run);
                        Console.WriteLine();

                        // Run a magic command
                        Console.WriteLine("Run a magic command:");
                        To_Run = $"%timeit sum(range(1000))";
                        shell.run_cell(To_Run);
                        Console.WriteLine();

                        // Run external Python scripts with arguments
                        Console.WriteLine("Run external Python scripts with arguments:");
                        To_Run =
                            $"\"{Path.Combine(pythonScriptsPath,
                            "add_numbers.py\" 5 7")}\"";
                        shell.run_line_magic("run", To_Run);
                        Console.WriteLine();

                        To_Run =
                            $"\"{Path.Combine(pythonScriptsPath,
                            "make_message.py\" Tim")}\"";
                        shell.run_line_magic("run", To_Run);
                        Console.WriteLine();

                        //To_Run =
                        //    $"\"{Path.Combine(pythonScriptsPath,
                        //    "autocompress.py\" -h")}\"";
                        //shell.run_line_magic("run", To_Run);
                        //Console.WriteLine();

                        To_Run =
                            $"\"{Path.Combine(pythonScriptsPath,
                            "Hello.py\" \"arg1\", \"arg with spaces\"")}\"";
                        shell.run_line_magic("run", To_Run);
                        Console.WriteLine();
                    }
                    finally
                    {
                        // Properly dispose the delegate wrapper before shutdown
                        if (systemDelegate != null)
                        {
                            shell.system = null;
                            systemDelegate.Dispose();
                        }
                    }
                }
            }
            finally
            {
                PythonEngine.Shutdown();
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        } // static void Main(/*string[] args*/)
    } // internal static class Program
} // namespace Embedded_IPython

