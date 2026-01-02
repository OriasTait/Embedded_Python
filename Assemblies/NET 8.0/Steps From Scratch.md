# Embedding Python and IPython in .NET 4.8 Applications
This document provides the steps to embed Python and IPython in .NET 8.0 applications.  This 
guide covers the following:
- Purpose of the embedded Python package
- How to obtain the Python package
- How to install the required packages
- How to set up your .NET application to use the embedded Python interpreter
- Copying the required files to your .NET application
- Setting up your .NET application to use the embedded Python

NOTES:
- These steps assume you are using Windows as your development environment.
- This guide assumes you have basic knowledge of .NET development and Python.
- This does not use any third-party libraries for embedding Python; instead,
  it relies on the standard Python distribution and .NET's ability to call external processes.

# Embedded Python Purpose
Embedded python available for download is meant for the following:
  "I just need Python to run a few pure-Python scripts with no external modules."

If you need anything more, the embedded ZIP **CANNOT WORK WITHOUT HEAVY MANUAL PATCHING**.

What *real* applications do (including commercial apps)
- Install a full Python version on the development machine
- Use that full Python installation to develop and test your scripts
- At deployment time, bundle the full Python installation inside your app
- At runtime, point the embedded Python to use the bundled full Python installation

# How to obtain the python package
- Download the full version from https://www.python.org/ftp/python/ (Using 3.13.7 for
  these steps)
  - In this case, Windows => python-3.13.7-amd64.exe
- Perform the installation on your development machine
  - Make sure to select "Add Python to PATH" during installation
- Verify the installation by running `python --version` in your command prompt
  - You should see the installed Python version (e.g., Python 3.13.7)
- Locate the installation directory (e.g., C:\Program Files\Python313)

# How to install the required packages on Windows
- Open Command Prompt *AS ADMINISTRATOR*
- Run the following commands to install the required packages:
  - `python -m pip install ipython`
- Verify the installation by running:
  - `python -m pip show ipython`
	- Looking for the Location: field to confirm the installation path
- Close the command prompt

# Copying the required files to your .NET application
- Create a folder named `py_e` in your project
- Copy the entire contents of the Python installation directory (e.g., C:\Program Files\Python313)
  into the `py_e` folder
- Create a folder named `py_scripts` in your project
- Copy your Python scripts into the `py_scripts` folder
- Ensure that the `py_e` and `py_scripts` folders are included in your project and set to 
  "Copy if newer" or "Copy always" in their properties
  - this is done by editing your .csproj file to include:
		<ItemGroup>
			<!-- embed runtime Python distribution -->
			<Content Include="py_e\**\*">
				<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
				<CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
			</Content>

			<!-- ensure python scripts are copied to build/publish output -->
			<Content Include="py_scripts\**\*">
				<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
				<CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
			</Content>
		</ItemGroup>

# Setting up your .NET application to use the embedded Python
- In your .NET application, set the Python home and path to point to the embedded Python directory
- example code snippet:
  // Initialize Python environment
  string baseDir = AppDomain.CurrentDomain.BaseDirectory;
  string pythonHome = Path.Combine(baseDir, "py_e");
  string pythonMyScripts = Path.Combine(baseDir, "py_scripts");
  string pythonLib = Path.Combine(pythonHome, "Lib");
  string pythonExe = Path.Combine(pythonHome, "python.exe");
  Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
  Environment.SetEnvironmentVariable("PYTHONPATH", pythonLib);
