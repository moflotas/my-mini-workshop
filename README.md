# MyUniversity

Table of contents:

- [Installation](#installation)
- [Code quality](#code-quality)
- [Testing](#testing)
- [Useful links](#useful-links)

# Installation

## Requirements

- DotNet SDK (not runtime!) 8.x: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- Some C# IDE: Rider (preferable) or Visual Studio (not Code)
- dotnet-ef tool: `dotnet tool install --global dotnet-ef`

## Instructions

Ask developers for `secrets.json` containing secrets for the app.

Put that file into the root of the solution.

Run the application with Rider runner or `dotnet run --project <ProjectName>`

## Code quality

Unlike Java, modern C# has built-in nullability handling.
`Data? data` means that variable `data` is nullable and should be null-checked.
Do not neglect nullability.

Use precise english names when naming and commenting. Add comments if your code is not self-explanatory.

There are **no checked exceptions** in C#.
If you are running an IO method, don't forget to check for Exceptions it may throw.

Write testable code: separate data access from its handling, keep functions simple, don't mix responsibilities.

Do not put main logic into classes that serve some other singular purpose (e.g. Controllers, Jobs, etc.)

Do not ignore warnings (suppress if irrelevant). Use `f2` key to jump to next warning in file (Rider with
IntelliJ bindings)

Use translations instead of inlining raw text in application.

If you are using Rider, consider installing CognitiveComplexity plugin.

Show your best!

## Testing

We are using NUnit library for testing. Keep tests as simple as possible.
During testing, be sceptical towards (your) code: try to find edge-cases.

In C#, tests are done in a different project that references the testing project.

To run tests use `dotnet test <ProjectName>` command.

For more
info:
- [MS Learn: Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
  (most examples are using xUnit, which differs from NUnit)
- [MS Learn: Unit testing C# with NUnit](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit)

# Useful links

- [Akaver's C# course](https://courses.taltech.akaver.com/csharp/lectures/01-oop-1/)
- [Akaver's Distributed course](https://courses.taltech.akaver.com/web-applications-with-csharp/lectures/01-http/)
- [ASP.NET Documentation](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
- [MS Learn: Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- Do not hesitate to ask maintainers for help!
