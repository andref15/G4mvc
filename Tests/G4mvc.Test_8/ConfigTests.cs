using G4mvc.Generator;
using G4mvc.TestBase.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace G4mvc.Test_8;

[TestClass]
public class ConfigTests
{
    private const string _testRootNamespace = "Test.Root.Namespace";

    [TestMethod]
    [DataRow(null, "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "null")]
    [DataRow("", "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "empty")]
    [DataRow("{}", "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "no properties")]
    [DataRow("""
    {
        "HelperClassName": "TestMVC"
    }
    """, "TestMVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "HelperClassName")]
    [DataRow("""
    {
        "LinksClassName": "TestLinks"
    }
    """, "MVC", "TestLinks", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "LinksClassName")]
    [DataRow("""
    {
        "StaticFilesPath": "testwwwroot"
    }
    """, "MVC", "Links", "testwwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "StaticFilesPath")]
    [DataRow("""
    {
        "UseVirtualPathProcessor": true
    }
    """, "MVC", "Links", "wwwroot", true, true, false, "global", false, null, null, null, null, null, DisplayName = "UseVirtualPathProcessor")]
    [DataRow("""
    {
        "UseVirtualPathProcessor": true,
        "UseProcessedPathForContentLink": true
    }
    """, "MVC", "Links", "wwwroot", true, true, false, "global", false, null, null, null, null, null, DisplayName = "UseVirtualPathProcessor and UseProcessedPathForContentLink = true")]
    [DataRow("""
    {
        "UseVirtualPathProcessor": true,
        "UseProcessedPathForContentLink": false
    }
    """, "MVC", "Links", "wwwroot", true, false, false, "global", false, null, null, null, null, null, DisplayName = "UseVirtualPathProcessor and UseProcessedPathForContentLink = false")]
    [DataRow("""
    {
        "UseProcessedPathForContentLink": true
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "only UseProcessedPathForContentLink = true")]
    [DataRow("""
    {
        "MakeGeneratedClassesInternal": true
    }
    """, "MVC", "Links", "wwwroot", false, false, true, "global", false, null, null, null, null, null, DisplayName = "MakeGeneratedClassesInternal")]
    [DataRow("""
    {
        "GeneratedClassNamespace": "global"
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, null, null, DisplayName = "GeneratedClassNamespace = global")]
    [DataRow("""
    {
        "GeneratedClassNamespace": "project"
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "project", false, null, null, null, null, _testRootNamespace, DisplayName = "GeneratedClassNamespace = project")]
    [DataRow("""
    {
        "GeneratedClassNamespace": "Test.Namespace"
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "Test.Namespace", false, null, null, null, null, "Test.Namespace", DisplayName = "GeneratedClassNamespace = \"Test.Namespace\"")]
    [DataRow("""
    {
        "GeneratedClassNamespace": ".TestNamespace"
    }
    """, "MVC", "Links", "wwwroot", false, false, false, ".TestNamespace", false, null, null, null, null, $"{_testRootNamespace}.TestNamespace", DisplayName = "GeneratedClassNamespace = \".TestNamespace\"")]
    [DataRow("""
    {
        "EnableSubfoldersInViews": true
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", true, null, null, null, null, null, DisplayName = "EnableSubfoldersInViews")]
    [DataRow("""
    {
        "ExcludedStaticFileExtensions": [
            "*.json",
            "*.txt"
        ]
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", false, new[] { "*.json", "*.txt" }, null, null, null, null, DisplayName = "EnableSubfoldersInViews")]
    [DataRow("""
    {
        "ExcludedStaticFileDirectories": [
            "/test1",
            "/test2"
        ]
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", false, null, new[] { "/test1", "/test2" }, null, null, null, DisplayName = "EnableSubfoldersInViews")]
    [DataRow("""
    {
        "AdditionalStaticFilesPaths": {
            "TestStaticFiles": "teststatic"
        }
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, new[] { "TestStaticFiles", "teststatic" }, null, null, DisplayName = "AdditionalStaticFilesPaths")]
    [DataRow("""
    {
        "CustomStaticFileDirectoryAlias": {
            "testdirectory": "testdir"
        }
    }
    """, "MVC", "Links", "wwwroot", false, false, false, "global", false, null, null, null, new[] { "testdirectory", "testdir" }, null, DisplayName = "CustomStaticFileDirectoryAlias")]
    [DataRow("""
    {
        "HelperClassName": "TestMVC",
        "LinksClassName": "TestLinks",
        "StaticFilesPath": "testwwwroot",
        "UseVirtualPathProcessor": true,
        "UseProcessedPathForContentLink": false,
        "MakeGeneratedClassesInternal": true,
        "GeneratedClassNamespace": "project",
        "EnableSubfoldersInViews": true,
        "ExcludedStaticFileExtensions": [
            "*.json",
            "*.txt"
        ],
        "ExcludedStaticFileDirectories": [
            "/test1",
            "/test2"
        ],
        "AdditionalStaticFilesPaths": {
            "TestStaticFiles": "teststatic"
        },
        "CustomStaticFileDirectoryAlias": {
            "testdirectory": "testdir"
        }
    }
    """, "TestMVC", "TestLinks", "testwwwroot", true, false, true, "project", true, new[] { "*.json", "*.txt" }, new[] { "/test1", "/test2" }, new[] { "TestStaticFiles", "teststatic" }, new[] { "testdirectory", "testdir" }, _testRootNamespace, DisplayName = "everything configured")]
    public void JsonConfigParsing(string configJson, string? helperClassName, string? linksClassName, string? staticFilesPath, bool useVirtualPathProcessor, bool? useProcessedPathForContentLink, bool makeGeneratedClassesInternal, string? generatedClassNamespace, bool enableSubfoldersInViews, string[]? excludedStaticFileExtensions, string[]? excludedStaticFileDirectories, string[]? additionalStaticFilesPaths, string[]? customStaticFileDirectoryAlias, string? expectedClassNamespace)
    {
        var config = new Configuration(LanguageVersion.Default, false, configJson, new("", _testRootNamespace));

        var jsonConfig = config.JsonConfig;

        Assert.AreEqual(helperClassName, jsonConfig.HelperClassName);
        Assert.AreEqual(linksClassName, jsonConfig.LinksClassName);
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
