using G4mvc.Generator.Compilation;
using G4mvc.TestBase.Extensions;
using Microsoft.CodeAnalysis.CSharp;

namespace G4mvc.TestBase;

public abstract class ConfigTests
{
    private const string _testRootNamespace = "Test.Root.Namespace";

    [TestMethod]
    [DataRow(null, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = "null")]
    [DataRow("", false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = "empty")]
    [DataRow("{}", false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = "no properties")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.DisableMvcHelperSourceGeneration)}}": true
    }
    """, true, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.DisableMvcHelperSourceGeneration))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.DisablePageHelperSourceGeneration)}}": true
    }
    """, false, true, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.DisablePageHelperSourceGeneration))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.DisableLinksHelperSourceGeneration)}}": true
    }
    """, false, false, true, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.DisableLinksHelperSourceGeneration))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.MvcHelperClassName)}}": "TestMVC"
    }
    """, false, false, false, "TestMVC", Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.MvcHelperClassName))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.PageHelperClassName)}}": "TestPages"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, "TestPages", Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.PageHelperClassName))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.LinksHelperClassName)}}": "TestLinks"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, "TestLinks", Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.LinksHelperClassName))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.StaticFilesPath)}}": "testwwwroot"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, "testwwwroot", false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.StaticFilesPath))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, true, true, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.UseVirtualPathProcessor))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true,
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": true
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, true, true, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = $"{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)} and {nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = true")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)}}": true,
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": false
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, true, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = $"{nameof(Configuration.JsonConfig.UseVirtualPathProcessor)} and {nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = false")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)}}": true
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = $"only {nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = true")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.MakeGeneratedClassesInternal)}}": true
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, true, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.MakeGeneratedClassesInternal))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "global"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, null, null, DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = global")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "project"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, "project", false, null, null, null, null, _testRootNamespace, DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = project")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": "Test.Namespace"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, "Test.Namespace", false, null, null, null, null, "Test.Namespace", DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = \"Test.Namespace\"")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.GeneratedClassNamespace)}}": ".TestNamespace"
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, ".TestNamespace", false, null, null, null, null, $"{_testRootNamespace}.TestNamespace", DisplayName = $"{nameof(Configuration.JsonConfig.GeneratedClassNamespace)} = \".TestNamespace\"")]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.EnableSubfoldersInViews)}}": true
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, true, null, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.EnableSubfoldersInViews))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.ExcludedStaticFileExtensions)}}": [
            "*.json",
            "*.txt"
        ]
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, new[] { "*.json", "*.txt" }, null, null, null, null, DisplayName = nameof(Configuration.JsonConfig.EnableSubfoldersInViews))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.ExcludedStaticFileDirectories)}}": [
            "/test1",
            "/test2"
        ]
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, new[] { "/test1", "/test2" }, null, null, null, DisplayName = nameof(Configuration.JsonConfig.EnableSubfoldersInViews))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.AdditionalStaticFilesPaths)}}": {
            "TestStaticFiles": "teststatic"
        }
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, new[] { "TestStaticFiles", "teststatic" }, null, null, DisplayName = nameof(Configuration.JsonConfig.AdditionalStaticFilesPaths))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.CustomStaticFileDirectoryAlias)}}": {
            "testdirectory": "testdir"
        }
    }
    """, false, false, false, Configuration.JsonConfigModel.DefaultMvcHelperClassName, Configuration.JsonConfigModel.DefaultPageHelperClassName, Configuration.JsonConfigModel.DefaultLinksHelperClassName, Configuration.JsonConfigModel.DefaultStaticFilesPath, false, false, false, Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, false, null, null, null, new[] { "testdirectory", "testdir" }, null, DisplayName = nameof(Configuration.JsonConfig.CustomStaticFileDirectoryAlias))]
    [DataRow($$"""
    {
        "{{nameof(Configuration.JsonConfig.DisableMvcHelperSourceGeneration)}}": true,
        "{{nameof(Configuration.JsonConfig.DisablePageHelperSourceGeneration)}}": true,
        "{{nameof(Configuration.JsonConfig.DisableLinksHelperSourceGeneration)}}": true,
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
    """, true, true, true, "TestMVC", "TestPages", "TestLinks", "testwwwroot", true, false, true, "project", true, new[] { "*.json", "*.txt" }, new[] { "/test1", "/test2" }, new[] { "TestStaticFiles", "teststatic" }, new[] { "testdirectory", "testdir" }, _testRootNamespace, DisplayName = "everything configured")]
    public void JsonConfigParsing(string configJson, bool disableMvcHelperSourceGeneration, bool disablePageHelperSourceGeneration, bool disableLinksHelperSourceGeneration, string? mvcHelperClassName, string? pageHelperClassName, string? linksHelperClassName, string? staticFilesPath, bool useVirtualPathProcessor, bool? useProcessedPathForContentLink, bool makeGeneratedClassesInternal, string? generatedClassNamespace, bool enableSubfoldersInViews, string[]? excludedStaticFileExtensions, string[]? excludedStaticFileDirectories, string[]? additionalStaticFilesPaths, string[]? customStaticFileDirectoryAlias, string? expectedClassNamespace)
    {
        var config = new Configuration(LanguageVersion.Default, false, configJson, new("", _testRootNamespace));

        var jsonConfig = config.JsonConfig;

        Assert.AreEqual(disableMvcHelperSourceGeneration, jsonConfig.DisableMvcHelperSourceGeneration);
        Assert.AreEqual(disablePageHelperSourceGeneration, jsonConfig.DisablePageHelperSourceGeneration);
        Assert.AreEqual(disableLinksHelperSourceGeneration, jsonConfig.DisableLinksHelperSourceGeneration);
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
