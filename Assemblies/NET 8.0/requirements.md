# Application Requirements

## Project Name
Embedded Python

## Tech Stack
- C# .NET 8
- SQLite (for simplicity)

## Core Features
1. Embed a Python interpreter within a .NET application.
2. Run simple, complex and magic Python scripts from within the .NET application.
3. Handle input and output between .NET and the embedded Python interpreter.
4. Provide error handling for Python script execution.
5. Allow loading Python scripts from external files.
6. Support asynchronous execution of Python scripts.
7. Log execution results and errors to a file.
8. Provide a simple UI (console or minimal GUI) to interact with the embedded Python functionality.
9. Ensure cross-platform compatibility (Windows, Linux, macOS).
10. Document the setup and usage of the embedded Python feature.
11. Implement unit tests for core functionalities.
12. Optimize performance for script execution.
13. Allow configuration of the Python environment (e.g., setting PYTHONPATH).
14. Support for passing complex data structures (like lists and dictionaries) between .NET and Python.

## Non-Functional Requirements
- Unit tests

## Naming Conventions
- Database tables: plural (e.g. Tickets)

## Output Requirements
Copilot should:
- Generate actual .cs files, not pseudo code.
- Follow layered architecture.
- Use async/await everywhere.