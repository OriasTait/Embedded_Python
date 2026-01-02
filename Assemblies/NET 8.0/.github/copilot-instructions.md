# Copilot Project Instructions
These instructions apply to **all Copilot Chat and inline completions** inside this repository.

## App Context
This repository contains an application defined in `requirements.md`.  
Always read and follow it before generating code.

## Rules for Code Generation
- Always generate real, runnable .NET 8.0 code.
- Align every file with the architecture described in requirements.md.
- When generating multi-file output, list each file with:
  - Full file path
  - Full file content
- Do not generate placeholder comments like "TODO".
- Always ensure code compiles.
- Include the directory py_e in the project root for the embedded Python distribution.
- Use wordwrap at 95 characters.
- Add comments to explain the modifications you made.

## C# Language and Formatting Rules

- Target framework: .NET 8.0
- Use classic, explicit C# syntax

### Namespace Rules
- Use block-scoped namespaces only
- Example:
  ```csharp
  namespace MyCompany.MyProduct.Services
  {
  }

## Architecture
- Controllers → Services → Repositories → DbContext → SQL
- Use Dependency Injection via the Program.cs builder.
- All services and repositories must be interfaces plus concrete implementations.

## Interaction Pattern
When asked to "generate" app modules:
1. Read requirements.md.
2. Generate the necessary files.
3. Summarize next steps.

## What Not to Do
- Do not invent your own architecture.
- Do not ignore naming conventions.
- Do not output partial code unless explicitly asked.
- Do not excede 95 characters per line.
- Do not remove existing comments.
