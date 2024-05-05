# G4mvc

G4mvc is a source generator for ASP.NET Core MVC apps that creates strongly typed helpers that eliminate the use of literal strings for routing.

It is an improved re-implementation of R4MVC using a C# Source Generator because R4MVC lacks support for many newer language - and MVC features.

[![CI](https://github.com/andref15/G4mvc/actions/workflows/ci.yml/badge.svg)](https://github.com/andref15/G4mvc/actions/workflows/ci.yml)

## Installation
Install the [G4mvc](https://www.nuget.org/packages/G4mvc/) and [G4mvc.Generator](https://www.nuget.org/packages/G4mvc.Generator/) NuGet packages.

To enable the use of the tag helpers for anchor and form tags, add the `@addTagHelper *, G4mvc` directive either in the view, or in the _ViewImports.cshtml to enable them globally.

If you want to use the `IUrlHelper` and `IHtmlHelper` Extension methods, add a using directive for `G4mvc.Extensions` either in the view, or in the _ViewImports.cshtml to enable them globally.

It might be necessary to restart Visual Studio for these changes to take affect.

## Use
### Source Generation
In order for the code generation to work, the controller class has to derive from the `Microsoft.AspNetCore.Mvc.Controller` class. Abstract classes will also be ignored. All Methods for which a routing helper should be generated, have to return `Microsoft.AspNetCore.Mvc.IActionResult`, `Microsoft.AspNetCore.Mvc.Infrastructure.IConvertToActionResult` or an implementation of either of these interfaces. For asyncronous controller actions, the task has to return one of these.\
To trigger the generation of the links class, it is necessary to manually build or rebuild the ASP.net core project or make change in the config file 

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
You can provide a JSON config file called g4mvc.json to change some of the defaults G4mvc uses.\
The json schema is available under https://raw.githubusercontent.com/andref15/G4mvc/main/Json/schema.json

    {
      "$schema": "https://raw.githubusercontent.com/andref15/G4mvc/main/Json/schema.json"
      "HelperClassName": "MVC",
      "LinksClassName":  "Links",
      "StaticFilesPath": "wwwroot",
      "UseVirtualPathProcessor": false,
      "UseProcessedPathForContentLink": false,
      "MakeGeneratedClassesInternal": false,
      "GeneratedClassNamespace": "global",
      "EnableSubfoldersInViews": false,
      "ExcludedStaticFileExtensions": [],
      "ExcludedStaticFileDirectories": [],
      "AdditionalStaticFilesPaths": {}
      "CustomStaticFileDirectoryAlias": {}
    }

#### HelperClassName
Allows you to change the MVC prefix (e.g. MVC.Home.Index())

#### LinksClassName
The class in which the links for static files are generated in

#### StaticFilesPath
The root path (relative to project dir) for which links will be generated

#### UseVirtualPathProcessor
Defines if you want to define a custom VirtualPathProcessor funcion. When this is set to true, all generated links will be static readonly instead of const fields and a partial class `VirtualPathProcessor` with a partial method `Process` will be generated and you have to write the implementation of this partial method.
An example of this can be seen here:

    internal static partial class VirtualPathProcessor
    {
        public static partial string Process(string path)
        {
            return path.ToUpper();
        }
    }

#### UseProcessedPathForContentLink
Defines if the processed path from the VirtualPathProcessor should be used for UrlHelper.Content methods.\
If this value is undefined and `UseVirtualPathProcessor` is set to true, this defaults to `true`.

#### MakeGeneratedClassesInternal
Defines if the generated route classes and the MVC and Links class will be public or internal

#### GeneratedClassNamespace
Defines in which namespace the generated MVC and Links class will exist. If this value is undefined, this defaults to `global`
- `global` defines that the files will exist in the global namespace (aka no namespace)'
- `project` defines that the files will exist in the root namespace of the web project
- any other value will be set as the namespace (eg. `G4mvc.SampleMVC`)
  - If the value starts with a `.` the namespace will be relative to the project root namespace (e.g. `.Generated`)

#### EnableSubfoldersInViews
Defines if subfolders in the controller's Views folder should be supported

#### ExcludedStaticFileExtensions
A list of file extensions that will be excluded from link generation

#### ExcludedStaticFileDirectories
A list of directories (relative to project dir) that will be excluded from link generation

#### AdditionalStaticFilesPaths
A dictionary of additional static file paths for which links will be generated
This is useful when you use the UseStaticFiles method serve files that are outside of the wwwroot folder. 
The key of this dictionary is the physical path relative to the project root and the value is the request path.

For the example provided in the [Microsoft Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-7.0#serve-files-outside-of-web-root) the AdditionalStaticFilesPaths configuration would look like this:

    "AdditionalStaticFilesPaths": {
        "MyStaticFiles": "StaticFiles"
    }

#### CustomStaticFileDirectoryAlias
A dictionary of aliases for certain directories (relative to project dir). This can be useful if the sanitized names of two or more subdirectories in a directory are the same and renaming them is not an option.
e.g. if you had a directory called `some-directory` and another one called `some.directory` in the `wwwroot` directory, the sanitized name of both of these would be `some_directory`, therefore the same class name would be generated twice.
To fix the situation described, a possible configuration would be the following:

    "CustomStaticFileDirectoryAlias": {
        "wwwroot/some.directory": "SomeDotDirectory"
    }
