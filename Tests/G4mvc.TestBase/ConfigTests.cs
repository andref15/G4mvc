using G4mvc.Generator.Compilation;
using G4mvc.TestBase.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace G4mvc.TestBase;

public abstract class ConfigTests
{
    private const string _testRootNamespace = "Test.Root.Namespace";

    [TestMethod]
    [DataRow(null, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "null")]
    [DataRow("", "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "empty")]
    [DataRow("{}", "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "no properties")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.MvcHelperClassName)}}": "TestMVC"
    }
    """, "TestMVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.MvcHelperClassName))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.PageHelperClassName)}}": "TestPages"
    }
    """, "MVC", "TestPages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.PageHelperClassName))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.LinksHelperClassName)}}": "TestLinks"
    }
    """, "MVC", "Pages", "TestLinks", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.LinksHelperClassName))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.StaticFilesPath)}}": "testwwwroot"
    }
    """, "MVC", "Pages", "Links", "testwwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.StaticFilesPath))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true
    }
    """, "MVC", "Pages", "Links", "wwwroot", true, true, false, "global", false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.UseVirtualPathProcessor))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true,
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": true
    }
    """, "MVC", "Pages", "Links", "wwwroot", true, true, false, "global", false, null, null, null, null, null, DisplayName = $"{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)} and {nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = true")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true,
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": false
    }
    """, "MVC", "Pages", "Links", "wwwroot", true, false, false, "global", false, null, null, null, null, null, DisplayName = $"{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)} and {nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = false")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": true
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = $"only {nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = true")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.MakeGeneratedClassesInternal)}}": true
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, true, "global", false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.MakeGeneratedClassesInternal))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "global"
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = global")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "project"
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "project", false, null, null, null, null, _testRootNamespace, DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = project")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "Test.Namespace"
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "Test.Namespace", false, null, null, null, null, "Test.Namespace", DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = \"Test.Namespace\"")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": ".TestNamespace"
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, ".TestNamespace", false, null, null, null, null, $"{_testRootNamespace}.TestNamespace", DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = \".TestNamespace\"")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.EnableSubfoldersInViews)}}": true
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", true, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.EnableSubfoldersInViews))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.ExcludedStaticFileExtensions)}}": [
            "*.json",
            "*.txt"
        ]
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, new[] { "*.json", "*.txt" }, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.EnableSubfoldersInViews))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.ExcludedStaticFileDirectories)}}": [
            "/test1",
            "/test2"
        ]
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, new[] { "/test1", "/test2" }, null, null, null, DisplayName = nameof(Configuration.JsonConfig.EnableSubfoldersInViews))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.AdditionalStaticFilesPaths)}}": {
            "TestStaticFiles": "teststatic"
        }
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, new[] { "TestStaticFiles", "teststatic" }, null, null, DisplayName = nameof(Configuration.JsonConfig.AdditionalStaticFilesPaths))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.CustomStaticFileDirectoryAlias)}}": {
            "testdirectory": "testdir"
        }
    }
    """, "MVC", "Pages", "Links", "wwwroot", false, false, false, "global", false, null, null, null, new[] { "testdirectory", "testdir" }, null, DisplayName = nameof(Configuration.JsonConfig.CustomStaticFileDirectoryAlias))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.MvcHelperClassName)}}": "TestMVC",
        "{{nameof(Configuration.JsonConfig.PageHelperClassName)}}": "TestPages",
        "{{nameof(Configuration.JsonConfig.LinksHelperClassName)}}": "TestLinks",
        "{{nameof(Configuration.JsonConfig.StaticFilesPath)}}": "testwwwroot",
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true,
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": false,
        "{{nameof(Configuration.JsonConfig.MakeGeneratedClassesInternal)}}": true,
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "project",
        "{{nameof(Configuration.JsonConfig.EnableSubfoldersInViews)}}": true,
        "{{nameof(Configuration.JsonConfig.ExcludedStaticFileExtensions)}}": [
            "*.json",
            "*.txt"
        ],
        "{{nameof(Configuration.JsonConfig.ExcludedStaticFileDirectories)}}": [
            "/test1",
            "/test2"
        ],
        "{{nameof(Configuration.JsonConfig.AdditionalStaticFilesPaths)}}": {
            "TestStaticFiles": "teststatic"
        },
        "{{nameof(Configuration.JsonConfig.CustomStaticFileDirectoryAlias)}}": {
            "testdirectory": "testdir"
        }
    }
    """, "TestMVC", "TestPages", "TestLinks", "testwwwroot", true, false, true, "project", true, new[] { "*.json", "*.txt" }, new[] { "/test1", "/test2" }, new[] { "TestStaticFiles", "teststatic" }, new[] { "testdirectory", "testdir" }, _testRootNamespace, DisplayName = "everything configured")]
    public void JsonConfigParsing(string configJson, string? mvcHelperClassName, string? pageHelperClassName, string? linksHelperClassName, string? staticFilesPath, bool useVirtualPathProcessor, bool? useProcessedPathForContentLink, bool makeGeneratedClassesInternal, string? generatedClassNamespace, bool enableSubfoldersInViews, string[]? excludedStaticFileExtensions, string[]? excludedStaticFileDirectories, string[]? additionalStaticFilesPaths, string[]? customStaticFileDirectoryAlias, string? expectedClassNamespace)
    {
        var config = new Configuration(LanguageVersion.Default, false, configJson, new("", _testRootNamespace));

        var jsonConfig = config.JsonConfig;

        Assert.AreEqual(mvcHelperClassName, jsonConfig.MvcHelperClassName);
        Assert.AreEqual(pageHelperClassName, jsonConfig.PageHelperClassName);
        Assert.AreEqual(linksHelperClassName, jsonConfig.LinksHelperClassName);
        Assert.AreEqual(staticFilesPath, jsonConfig.StaticFilesPath);
        Assert.AreEqual(useVirtualPathProcessor, jsonConfig.UseVirtualPathProcessor);
        Assert.AreEqual(useProcessedPathForContentLink, jsonConfig.UseProcessedPathForContentLink);
        Assert.AreEqual(makeGeneratedClassesInternal, jsonConfig.MakeGeneratedClassesInternal);
        Assert.AreEqual(generatedClassNamespace, jsonConfig.GeneratedClassNamespace);
        Assert.AreEqual(enableSubfoldersInViews, jsonConfig.EnableSubfoldersInViews);
        CollectionAssert.AreEquivalent(excludedStaticFileExtensions, jsonConfig.ExcludedStaticFileExtensions);
        CollectionAssert.AreEquivalent(excludedStaticFileDirectories, jsonConfig.ExcludedStaticFileDirectories);
        CollectionAssert.That.AreDictionariesEquivalent(additionalStaticFilesPaths, jsonConfig.AdditionalStaticFilesPaths);
        CollectionAssert.That.AreDictionariesEquivalent(customStaticFileDirectoryAlias, jsonConfig.CustomStaticFileDirectoryAlias);

        Assert.AreEqual(expectedClassNamespace, config.GeneratedClassNamespace);
        Assert.AreEqual(makeGeneratedClassesInternal ? "internal" : "public", config.GeneratedClassModifier);
    }
}
