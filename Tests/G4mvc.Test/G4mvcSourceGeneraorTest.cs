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
using System.Text.Json;

namespace G4mvc.Test;

internal class G4mvcSourceGeneraorTest : CSharpSourceGeneratorTest<G4mvcGenerator, DefaultVerifier>
{
    private static readonly ReferenceAssemblies _referenceAssemblies = new ReferenceAssemblies($"net{Environment.Version.Major}.0", new PackageIdentity("Microsoft.AspNetCore.App.Ref", Environment.Version.ToString(3)), Path.Combine("ref", $"net{Environment.Version.Major}.0")).AddPackages([
            new PackageIdentity("Microsoft.NETCore.App.Ref", Environment.Version.ToString(3))
        ]);

    public G4mvcSourceGeneraorTest()
    {
        ReferenceAssemblies = _referenceAssemblies;
        CompilerDiagnostics = CompilerDiagnostics.Suggestions;

    }

    public async Task InitializeAsync(Configuration.JsonConfigModel? jsonConfig, ExpectedOutputs expectedOutputs, CancellationToken cancellationToken)
    {
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

    protected override CompilationOptions CreateCompilationOptions()
    {
        return new CSharpCompilationOptions(OutputKind.ConsoleApplication, nullableContextOptions: NullableContextOptions.Enable);
    }
}
