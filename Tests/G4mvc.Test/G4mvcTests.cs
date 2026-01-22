using G4mvc.Generator.Compilation;
using G4mvc.Test.OutputComparisson;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace G4mvc.Test;

[TestClass]
public sealed class G4mvcTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task DefaultOptions()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create().Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_ClassNames()
    {
        const string mvc = "TestMvc";
        const string pages = "TestRazorPages";
        const string links = "TestLinks";

        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .WithMvcClassName(mvc)
            .WithPageHelperClassName(pages)
            .WithLinksClassName(links)
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    [DataRow(null, DisplayName = $"{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = null")]
    [DataRow(true, DisplayName = $"{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = true")]
    [DataRow(false, DisplayName = $"{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = false")]
    public async Task CustomOptions_UseVirtualPathProcessor(bool? useProcessedPathForContentLink)
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .UseVirtualPathProcessor()
            .UseVirtualPathProcessorForContentLinks(useProcessedPathForContentLink ?? true)
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);
        context.TestState.Sources.Add(("", SourceText.From("""
            internal static partial class VirtualPathProcessor
            {
                public static partial string Process(string path)
                {
                    return path;
                }
            }
            """, Encoding.UTF8)));

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_MakeGeneratedClassesInternal()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .MakeGeneratedClassesInternal()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_GeneratedClassNamespace()
    {
        const string classNamespace = $"{nameof(G4mvc)}.{nameof(Test)}.GeneratedFiles";

        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .WithClassNamespace(classNamespace)
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_EnableSubfoldersInViews()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .EnumerateSubDirectories()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_StaticFilesPath()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .UseAlternativeRoot()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_ExcludedStaticFileExtensions()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .ExcludeIco()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_ExcludedStaticFileDirectories()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .ExcludeCss()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_AdditionalStaticFilesPaths()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .UseAdditionalStaticFilesPath()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task CustomOptions_CustomStaticFileDirectoryAlias()
    {
        var context = new G4mvcSourceGeneraorTest();
        var (config, expectedOutputs) = ConfigAndExpectedOutputBuilder.Create()
            .UseCustomJsName()
            .Build();

        await context.InitializeAsync(config, expectedOutputs, TestContext.CancellationToken);

        await context.RunAsync(TestContext.CancellationToken);
    }
}
