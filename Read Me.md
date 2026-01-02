# Overview of the solution

This solution demonstrates embedding a Python runtime from a local `py_e` distribution
and invoking Python scripts from managed C#, with parallel implementations targeting
both .NET Framework 4.8 and .NET 8.0. The focus is on reliable process execution,
safe command quoting, and a clear, service‑oriented structure.

Shared capabilities (applies to both .NET 4.8 and .NET 8.0):
- Uses the `py_e` directory at the repository root for the embedded Python distribution.
- Initializes environment variables (e.g., `PYTHONHOME`, `PYTHONPATH`) for the Python
  runtime to ensure consistent behavior.
- Invokes Python via `System.Diagnostics.Process` with stdout/stderr capture, avoiding
  deadlocks by reading both streams and waiting for process exit.
- Provides helpers for safe command construction and quoting to prevent shell injection
  and argument parsing issues.
- Encourages a layered, service‑oriented approach (Controllers → Services → Repositories
  → DbContext → SQL) and dependency injection via `Program.cs`, keeping responsibilities
  clear and testable.

.NET Framework 4.8 implementation:
- Entry point and utilities are under `Assemblies/NET 4.8/NET 4.8/Program/`.
- `Program.cs` wires up dependencies and orchestrates Python invocations.
- `QuoteForCmd.cs` provides Windows‑safe quoting/escaping for command‑line arguments.

.NET 8.0 implementation:
- Entry point is under `Assemblies/NET 8.0/NET 8.0/Program/`.
- `Program.cs` demonstrates:
  - Setting Python environment variables based on the app base directory.
  - Running inline Python (`-c`) and external scripts (e.g., `hello.py`, `Dynamic.py`).
  - Quoting script paths and arguments using a safe `QuoteForCmd` helper.
  - Creating script directories on demand and writing dynamic Python files.
  - Reading stdout/stderr and printing process exit codes.
- `QuoteForCmd.cs` centralizes argument quoting for .NET 8.0.

Project structure highlights:
- `Assemblies/NET 4.8/NET 4.8/Program/Program.cs`: .NET 4.8 entry point and DI wiring.
- `Assemblies/NET 4.8/NET 4.8/Program/QuoteForCmd.cs`: Quoting utilities for .NET 4.8.
- `Assemblies/NET 8.0/NET 8.0/Program/Program.cs`: .NET 8.0 entry point showing Python
  process execution patterns and dynamic script creation.
- `Assemblies/NET 8.0/NET 8.0/Program/QuoteForCmd.cs`: Quoting utilities for .NET 8.0.

How it works (both .NET 4.8 and .NET 8.0):
1. The application locates the embedded Python distribution under `py_e` at runtime.
2. `Program.cs` sets environment variables and constructs `ProcessStartInfo` for Python.
3. Scripts are invoked either inline (`-c`) or via files, with safe quoting of paths
   and arguments.
4. The application captures stdout/stderr, waits for exit, and reports exit codes.

What you can do next:
- Place your Python scripts alongside or within `py_e` and reference them from C#.
- Extend services to encapsulate Python interactions (process start, stream capture,
  result handling) and use DI to wire them.
- Add repositories if Python interactions involve I/O (files, databases) to isolate
  persistence concerns.

Rationale:
This design keeps .NET concerns (DI, services, repositories) separated from process
management while making Python integration explicit and maintainable. It ensures
robust command quoting and allows shipping the Python runtime with the application
for consistent deployments across both .NET Framework 4.8 and .NET 8.0.
