# Embedded_Python
A template that can be used for using embedded python for .NET 8.0 applications.

# Embedded Python Purpose
Embedded python available for download is meant for the following:
  “I just need Python to run a few pure-Python scripts with no external modules.”

If you need anything more, the embedded ZIP **CANNOT WORK WITHOUT HEAVY MANUAL PATCHING**.

What *real* applications do (including commercial apps)
- Bundle a full Python installation inside your app
  - Download the full version from https://www.python.org/ftp/python/  (Using 3.13.7 for these steps)
  - Obtain the full working version for the OS you are developing on
	(In this case, Windows => python-3.13.7-amd64.zip)

# How to obtain the python package
1. Download the full version from https://www.python.org/ftp/python/ (Using 3.13.7 for these steps)
2. Extract the ZIP to a temporary folder
4. Create a folder named `py_e` in the root of your project folder
  - The name `py_e` is used because it is short for "Python Embedded".  Windows has a path length
	limitation of 260 characters, so shorter folder names help.
5. Move the entire contents of the extracted python to the root of `py_e` folder

# Obtain IPython
Step 1: Create venv using your bundled python
- Open a command prompt
- cd MyDotNetApp\py_e
- .\python.exe -m venv venv

Step 2: Activate the correct environment
- venv\Scripts\activate

Verify:
- where python
- python --version

Step 3: Install IPython (NO --target)
- python -m pip install --upgrade pip setuptools wheel
- python -m pip install ipython

Step 4: Disable user site-packages
- set PYTHONNOUSERSITE=1

Step 4: Verify isolation
- python -c "import IPython, inspect; print(inspect.getfile(IPython))"
- python -c "import IPython; print(IPython.__file__)"

# =============

# Add Nuget Packages => Is this needed?
- pythonnet by Conan, denfromufa, pythonnet

# =============

# Update the project file
Add the following to the bottom of your .csproj file right
before <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
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
