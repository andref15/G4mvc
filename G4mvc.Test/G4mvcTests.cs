using G4mvc.Generator;
using G4mvc.Generator.Compilation;
using G4mvc.Generator.Helpers;
using G4mvc.Test.OutputComparisson;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using System.Text.Json;

namespace G4mvc.Test;

[TestClass]

public sealed class G4mvcTests
{
    private static ReferenceAssemblies _referenceAssemblies = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        var netVersion = Environment.Version;

        _referenceAssemblies = new ReferenceAssemblies($"net{netVersion.Major}.0", new PackageIdentity("Microsoft.NETCore.App.Ref", netVersion.ToString(3)), Path.Combine("ref", $"net{netVersion.Major}.0"));
    }

    private async Task InitializeAsync(Configuration.JsonConfigModel? jsonConfig = null)
    {
        var context = new CSharpSourceGeneratorTest<G4mvcGenerator, DefaultVerifier>
        {
            ReferenceAssemblies = _referenceAssemblies,
            CompilerDiagnostics = CompilerDiagnostics.Suggestions
        };

        if (jsonConfig.HasValue)
        {
            context.TestState.AdditionalFiles.Add((Configuration.FileName, JsonSerializer.Serialize(jsonConfig.Value)));
        }

        context.TestState.AnalyzerConfigFiles.Add((Path.Combine(Environment.CurrentDirectory, ".analyzerconfig"), $"""
            is_global = true
            {GlobalOptionConstant.BuildProperty.ProjectDir} = {Environment.CurrentDirectory}
            """));

        var rootDir = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var file in rootDir.EnumerateFiles("*.cs", SearchOption.AllDirectories))
        {
            using var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);
            context.TestState.Sources.Add((file.Name, await sr.ReadToEndAsync()));
        }

        foreach (var file in rootDir.EnumerateFiles("*.cshtml", SearchOption.AllDirectories))
        {
            using var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            using var sr = new StreamReader(fs);
            context.TestState.Sources.Add((file.Name, await sr.ReadToEndAsync()));
        }

        context.TestState.GeneratedSources.AddExpectedOutputs(new ExpectedOutputs());

        await context.RunAsync();
    }

    [TestMethod]
    public async Task DefaultOptions()
    {
        await InitializeAsync();

    }
}
