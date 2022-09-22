# G4mvc

G4mvc is a source generator for ASP.NET Core MVC apps that creates strongly typed helpers that eliminate the use of literal strings in many places.

It is a re-implementation of R4MVC using a C# Source Generator.

## Installation
Install the [G4mvc](https://www.nuget.org/packages/G4mvc/) and [G4mvc.Generator](https://www.nuget.org/packages/G4mvc.Generator/) NuGet packages.

To enable the use of the tag helpers for anchor and form tags, add the `@addTagHelper *, G4mvc` directive either in the view, or in the _ViewImports.cshtml to enable them globally.

If you want to use the `IUrlHelper` and `IHtmlHelper` Extension methods, add a using directive for `G4mvc.Extensions` either in the view, or in the _ViewImports.cshtml to enable them globally.

It might be necessary to restart Visual Studio for these changes to take affect.

## Use
### Source Generation
In order for the code generation to work, the controller class has to derive from the `Microsoft.AspNetCore.Mvc.Controller` class. Abstract classes will also be ignored. All Methods for which a routing helper should be generated, have to return `Microsoft.AspNetCore.Mvc.IActionResult`, `Microsoft.AspNetCore.Mvc.Infrastructure.IConvertToActionResult` or an implementation of either of these interfaces. For asyncronous controller actions, the task has to return one of these.

Examples:
`public IActionResult Edit(EditViewModel viewModel)`
`public JsonResult Edit(EditViewModel viewModel)`
`public Task<IActionResult> Edit(EditViewModel viewModel)`
`public Task<JsonResult> Edit(EditViewModel viewModel)`
`public IConvertToActionResult Edit(EditViewModel viewModel)`
`public ActionResult<IEnumerable<string>> Edit(EditViewModel viewModel)`

Something like `public IEnumerable<string> Edit(EditViewModel viewModel)` would be ignored.

### Extensions
The [G4mvc](https://www.nuget.org/packages/G4mvc/) package provides a number of extension methods that can make using the route helpers a bit easier. You do however not have to rely on these because the `G4mvcRouteValues` class derives from the standard `Microsoft.AspNetCore.Routing.RouteValueDictionary`, so you can use any ASP.net Core Methods provided by Microsoft.
