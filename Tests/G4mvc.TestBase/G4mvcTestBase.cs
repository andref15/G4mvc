using G4mvc.Generator;
using G4mvc.TestBase.Providers;
using G4mvc.TestBase.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;

namespace G4mvc.TestBase;
public abstract class G4mvcTestBase(LanguageVersion languageVersion)
{
    protected readonly CSharpCompilationOptions CompilationOptions = new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release, warningLevel: 0, nullableContextOptions: NullableContextOptions.Enable);

    protected readonly CSharpParseOptions ParseOptions = CSharpParseOptions.Default.WithLanguageVersion(languageVersion).WithDocumentationMode(DocumentationMode.None);

    protected static void AssertDiagnostics(CSharpCompilation compilation, string type)
    {
        var diagnostics = compilation.GetDiagnostics();
        Assert.AreEqual(0, diagnostics.Length, "{0} classes have diagnostic messages:\n{1}", type, string.Join('\n', diagnostics));
    }

    protected static IEnumerable<MetadataReference> GetMetadataReferences()
    {
        var msAssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;

        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "mscorlib.dll"));
        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "System.dll"));
        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "System.Core.dll"));
        yield return MetadataReference.CreateFromFile(Path.Combine(msAssemblyPath, "System.Runtime.dll"));

        yield return MetadataReference.CreateFromFile(typeof(System.Runtime.JitInfo).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.Controller).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.Core.Infrastructure.IAntiforgeryValidationFailedResult).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Http.HttpContext).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Microsoft.Extensions.Logging.ILogger).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.IActionResult).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(System.Diagnostics.Activity).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonSerializer).Assembly.Location);
        yield return MetadataReference.CreateFromFile(typeof(G4mvcRouteValues).Assembly.Location);
    }

    private protected CSharpCompilation BaseTest(Configuration.JsonConfigModel? jsonConfig = null, IEnumerable<SyntaxTree>? additionalSyntaxTrees = null)
    {
        var syntaxTrees = GetSyntaxTreesInBuildDirectory();

        if (additionalSyntaxTrees is not null)
        {
            syntaxTrees = syntaxTrees.Concat(additionalSyntaxTrees);
        }

        var g4mvcGenerator = new G4mvcGenerator();

        var compilation = CSharpCompilation.Create("TestWeb", syntaxTrees, GetMetadataReferences(), options: CompilationOptions);

        var generatorDriver = CSharpGeneratorDriver.Create(g4mvcGenerator).WithUpdatedParseOptions(ParseOptions).WithUpdatedAnalyzerConfigOptions(new G4mvcAnalyzerConfigOptionsProvider());

        if (jsonConfig.HasValue)
        {
            generatorDriver = generatorDriver.AddAdditionalTexts([new ConfigAdditionalText(jsonConfig.Value)]);
        }

        generatorDriver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilationBase, out _);

        Assert.IsInstanceOfType<CSharpCompilation>(outputCompilationBase);
        var outputCompilation = (CSharpCompilation)outputCompilationBase;

        AssertDiagnostics(outputCompilation, "Output");

        return outputCompilation;
    }

    protected void AssertExpectedSyntaxTrees(ExpectedOutputsBase expectedOutputs, ImmutableArray<SyntaxTree> syntaxTrees, int expectedCount = 9)
    {
        Assert.AreEqual(expectedCount, syntaxTrees.Length);

        try
        {
            SyntaxAssert.AreAnyEquivalent(expectedOutputs.TestRoutesClass, syntaxTrees, ParseOptions);
            SyntaxAssert.AreAnyEquivalent(expectedOutputs.TestPartialRoutesClass, syntaxTrees, ParseOptions);
            SyntaxAssert.AreAnyEquivalent(expectedOutputs.TestPartialClass, syntaxTrees, ParseOptions);
            SyntaxAssert.AreAnyEquivalent(expectedOutputs.SharedClass, syntaxTrees, ParseOptions);
            SyntaxAssert.AreAnyEquivalent(expectedOutputs.MvcClass, syntaxTrees, ParseOptions);
            SyntaxAssert.AreAnyEquivalent(expectedOutputs.LinksClass, syntaxTrees, ParseOptions);
        }
        catch
        {
            Console.WriteLine("EXPECTED:\n");
            Console.WriteLine(expectedOutputs.TestRoutesClass);
            Console.WriteLine(expectedOutputs.TestPartialRoutesClass);
            Console.WriteLine(expectedOutputs.TestPartialClass);
            Console.WriteLine(expectedOutputs.SharedClass);
            Console.WriteLine(expectedOutputs.MvcClass);
            Console.WriteLine(expectedOutputs.LinksClass);
            Console.WriteLine("\nEND EXPECTED\n");

            Console.WriteLine("ACTUAL:");

            foreach (var syntaxTree in syntaxTrees.Where(s => s.FilePath.Length > 0))
            {
                Console.WriteLine(syntaxTree);
            }

            Console.WriteLine("\nEND ACTUAL\n");

            throw;
        }
    }

    protected IEnumerable<SyntaxTree> GetSyntaxTreesInBuildDirectory()
    {
        var directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);

        foreach (var file in directoryInfo.EnumerateFiles("*.cs", SearchOption.AllDirectories).OrderBy(f => f.Name))
        {
            using var stream = file.OpenRead();
            yield return SyntaxUtils.ToSyntaxTree(stream, ParseOptions);
        }
    }

    protected bool SyntaxTreeIsEquivalentTo(SyntaxTree left, string right)
        => SyntaxUtils.AreEquivalent(left, right, ParseOptions);

}
