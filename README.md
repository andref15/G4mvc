# G4mvc

G4mvc is a source generator for ASP.NET Core MVC apps that creates strongly typed helpers that eliminate the use of literal strings in many places.

It is an improved re-implementation of R4MVC using a C# Source Generator because R4MVC lacks support for many newer language and MVC features.

## Installation
Install the [G4mvc](https://www.nuget.org/packages/G4mvc/) and [G4mvc.Generator](https://www.nuget.org/packages/G4mvc.Generator/) NuGet packages.

To enable the use of the tag helpers for anchor and form tags, add the `@addTagHelper *, G4mvc.TagHelpers` directive either in the view, or in the _ViewImports.cshtml to enable them globally.

If you want to use the `IUrlHelper` and `IHtmlHelper` Extension methods, add a using directive for `G4mvc.Extensions` either in the view, or in the _ViewImports.cshtml to enable them globally.

It might be necessary to restart Visual Studio for these changes to take affect.

## Use
### Source Generation
In order for the code generation to work, the controller class has to derive from the `Microsoft.AspNetCore.Mvc.Controller` class. Abstract classes will also be ignored. All Methods for which a routing helper should be generated, have to return `Microsoft.AspNetCore.Mvc.IActionResult`, `Microsoft.AspNetCore.Mvc.Infrastructure.IConvertToActionResult` or an implementation of either of these interfaces. For asyncronous controller actions, the task has to return one of these.

#### Examples:
    public IActionResult Edit(EditViewModel viewModel)
    public JsonResult Edit(EditViewModel viewModel)
    public Task<IActionResult> Edit(EditViewModel viewModel)
    public Task<JsonResult> Edit(EditViewModel viewModel)
    public IConvertToActionResult Edit(EditViewModel viewModel)
    public ActionResult<IEnumerable<string>> Edit(EditViewModel viewModel)

Something like `public IEnumerable<string> Edit(EditViewModel viewModel)` would be ignored.

### Extensions
The [G4mvc](https://www.nuget.org/packages/G4mvc/) package provides a number of extension methods that can make using the generated route helpers a bit easier. You do however not have to rely on these because the `G4mvcRouteValues` class derives from the standard `Microsoft.AspNetCore.Routing.RouteValueDictionary`, so you can use any of the methods provided by ASP.net Core, that have an `object routeValues` parameter. An example would be the [HtmlHelper.RouteLink Method](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.viewfeatures.htmlhelper.routelink?view=aspnetcore-6.0).
The provided extension methods are just wrappers for these methods.

### Tag Helpers
G4mvc provides a TagHelper that can be used on anchor as well as form tags.

`<a g4-action="MVC.Home.Index()">Home</a>`

### Configuration
You can provide a JSON config file called g4mvc.json to change some of the defaults G4mvc uses.
    {
      "HelperClassName": "MVC",
      "LinksClassName":  "Links",
      "StaticFilesPath": "wwwroot",
      "ExcludedStaticFileExtensions": [],
      "ExcludedStaticFileDirectories": [],
      "AdditionalStaticFilesPaths": {}
    }

This configuration file needs to be added to the compilation context by adding it as an additional file in the csproj file:

    <ItemGroup>
        <AdditionalFiles Include="g4mvc.json" />
    </ItemGroup>

#### HelperClassName
Allows you to change the MVC prefix (e.g. MVC.Home.Index())

#### LinksClassName
The class in which the links for static files are generated in

#### StaticFilesPath
The root path for which links will be generated

#### ExcludedStaticFileExtensions
A list of file extensions that will be excluded from link generation

#### ExcludedStaticFileDirectories
A list of directories that will be excluded from link generation

#### AdditionalStaticFilesPaths
A dictionary of additional static file paths for which links will be generated
This is useful when you use the UseStaticFiles method serve files that are outside of the wwwroot folder. The key of this dictionary is the request path and the value is the physical path relative to the project root.

For the example provided in the [Microsoft Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-7.0#serve-files-outside-of-web-root) the AdditionalStaticFilesPaths configuration would look like this:

    "AdditionalStaticFilesPaths": {
        "StaticFiles": "MyStaticFiles"
    }
