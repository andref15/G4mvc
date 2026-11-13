using G4mvc.Generator;
using G4mvc.Generator.Compilation;
using G4mvc.Generator.Helpers;
using G4mvc.Test.OutputComparisson;
using G4mvc.Test.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Model;
using System.Collections.Immutable;
using System.Text.Json;

namespace G4mvc.Test;

internal class G4mvcSourceGeneratorTest : CSharpSourceGeneratorTest<G4mvcGenerator, DefaultVerifier>
{
    private static readonly ReferenceAssemblies _referenceAssemblies = new ReferenceAssemblies(EnvironmentVersion.NetVersion, new PackageIdentity("Microsoft.AspNetCore.App.Ref", EnvironmentVersion.VersionString), Path.Combine("ref", EnvironmentVersion.NetVersion)).AddPackages([
            new PackageIdentity("Microsoft.NETCore.App.Ref", EnvironmentVersion.VersionString)
        ]);

    public G4mvcSourceGeneratorTest()
    {
        ReferenceAssemblies = _referenceAssemblies;
        CompilerDiagnostics = CompilerDiagnostics.Suggestions;

    }

    public async Task InitializeAsync(Configuration.JsonConfigModel? jsonConfig, ExpectedOutputs expectedOutputs, CancellationToken cancellationToken)
    {
        //TestBehaviors |= TestBehaviors.SkipGeneratedSourcesCheck;

        TestState.OutputKind = OutputKind.ConsoleApplication;

        TestState.AdditionalReferences.Add(typeof(G4mvcBaseRouteValues<>).Assembly.Location);

        if (jsonConfig.HasValue)
        {
            TestState.AdditionalFiles.Add((Configuration.FileName, JsonSerializer.Serialize(jsonConfig.Value)));
        }

        TestState.AnalyzerConfigFiles.Add((Path.Combine(Environment.CurrentDirectory, ".analyzerconfig"), $"""
            is_global = true
            {GlobalOptionConstant.BuildProperty.ProjectDir} = {Environment.CurrentDirectory}
            {GlobalOptionConstant.BuildProperty.RootNamespace} = {nameof(G4mvc)}.{nameof(Test)}
            """));

        var rootDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        DisabledDiagnostics.Add("CS1591");

        TestState.Sources.AddEntrypoint(rootDirectory);
        await TestState.AdditionalFiles.AddCshtmlFilesAsync(rootDirectory, cancellationToken);
        await TestState.Sources.AddCsFilesAsync(rootDirectory, cancellationToken);
        TestState.GeneratedSources.AddExpectedOutputs(expectedOutputs);
        OptionsTransforms.Add((options) => options
            .WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, false)
            .WithChangedOption(FormattingOptions.SmartIndent, LanguageNames.CSharp, FormattingOptions.IndentStyle.Smart)
            .WithChangedOption(FormattingOptions.IndentationSize, LanguageNames.CSharp, 4));
    }

    protected override async Task RunImplAsync(CancellationToken cancellationToken)
    {
        var analyzers = GetDiagnosticAnalyzers().ToArray();
        var defaultDiagnostic = GetDefaultDiagnostic(analyzers);
        var supportedDiagnostics = analyzers.SelectMany(analyzer => analyzer.SupportedDiagnostics).ToImmutableArray();
        var fixableDiagnostics = ImmutableArray<string>.Empty;
        var testState = TestState.WithInheritedValuesApplied(null, fixableDiagnostics).WithProcessedMarkup(MarkupOptions, defaultDiagnostic, supportedDiagnostics, fixableDiagnostics, DefaultFilePath);

        var project = await CreateProjectAsync(new EvaluatedProjectState(testState, ReferenceAssemblies), testState.AdditionalProjects.Values.Select(additionalProject => new EvaluatedProjectState(additionalProject, ReferenceAssemblies)).ToImmutableArray(), cancellationToken);


        var (compilation, diagnostics) = await base.GetProjectCompilationAsync(project, Verify, cancellationToken);

        var numOriginalSources = testState.Sources.Count;
        var projectSyntaxTrees = compilation.SyntaxTrees.Take(numOriginalSources).ToDictionary(st => st.FilePath, st => st);
        var generatedSyntaxTrees = compilation.SyntaxTrees.Skip(numOriginalSources).ToDictionary(st => st.FilePath, st => st);

        await VerifyProjectSyntaxTreesAsync(testState.Sources, projectSyntaxTrees);
        await VerifyGeneratedSyntaxTreesAsync(testState.GeneratedSources, generatedSyntaxTrees);
        await VerifyDiagnosticsAsync(new EvaluatedProjectState(testState, ReferenceAssemblies), testState.AdditionalProjects.Values.Select(additionalProject => new EvaluatedProjectState(additionalProject, ReferenceAssemblies)).ToImmutableArray(), testState.ExpectedDiagnostics.ToArray(), Verify, cancellationToken).ConfigureAwait(false);

        Console.WriteLine();
    }

    private async Task VerifyProjectSyntaxTreesAsync(SourceFileCollection expected, Dictionary<string, SyntaxTree> actual)
    {
        Verify.Equal(expected.Count, actual.Count, "Expected source file list to match");

        await VerifyFilesAsync(Verify.PushContext("Original files after running source generators"), expected, actual);
    }
    private static async Task VerifyGeneratedSyntaxTreesAsync(SourceFileCollection expected, Dictionary<string, SyntaxTree> actual)
    {
        Verify.Equal(expected.Count, actual.Count, "Expected source file list to match");

        await VerifyFilesAsync(Verify.PushContext("Verifying source generated files"), expected, actual);
    }

    private static async Task VerifyFilesAsync(IVerifier verifyContext, SourceFileCollection expected, Dictionary<string, SyntaxTree> actual)
    {
        foreach (var expectedSource in expected)
        {
            verifyContext.True(actual.TryGetValue(expectedSource.filename, out var actualSource), "");

            verifyContext.EqualOrDiff(expectedSource.content.ToString(), actualSource.ToString(), "Expected source file list to match");
        }
    }

    protected override CompilationOptions CreateCompilationOptions()
    {
        return new CSharpCompilationOptions(OutputKind.ConsoleApplication, nullableContextOptions: NullableContextOptions.Enable);
    }

    protected override ParseOptions CreateParseOptions()

            => new CSharpParseOptions(
#if NET11_0_OR_GREATER //until update is released
                LanguageVersion.Preview
#else
                LanguageVersion.Default
#endif
                , DocumentationMode.Diagnose, SourceCodeKind.Regular, [
#if NET10_0_OR_GREATER
                "NET10_0_OR_GREATER",
#if NET10_0
                "NET10_0",
#endif
#endif
#if NET11_0_OR_GREATER
                "NET11_0_OR_GREATER",
#if NET11_0
                "NET11_0",
#endif
#endif
            ]);
}
