# G4mvc

G4mvc is a source generator for ASP.NET Core MVC apps that creates strongly typed helpers that eliminate the use of literal strings in many places.

It is a re-implementation of R4MVC using a C# Source Generator.

## Installation
Install the [G4mvc](https://www.nuget.org/packages/G4mvc/) and [G4mvc.Generator](https://www.nuget.org/packages/G4mvc.Generator/) NuGet packages.

To enable the use of the tag helpers for anchor and form tags, add the `@addTagHelper *, G4mvc` directive either in the view, or in the _ViewImports.cshtml to enable them globally.

If you want to use the `IUrlHelper` and `IHtmlHelper` Extension methods, add a using directive for `G4mvc.Extensions` either in the view, or in the _ViewImports.cshtml to enable them globally.

It might be necessary to restart Visual Studio for these changes to take affect.

## Use
