using G4mvc.Generator.Compilation;
using G4mvc.Generator.SourceEmitters;
using Microsoft.CodeAnalysis.Diagnostics;

namespace G4mvc.Generator;

[Generator(LanguageNames.CSharp)]
public class G4mvcGenerator : IIncrementalGenerator
{
    private readonly LinksGenerator _linksGenerator = new();
    private readonly ControllerGenerator _controllerGenerator = new();
    private readonly PagesGenerator _pagesGenerator = new();

    public delegate void RegisterSourceOutputDelegate<TSource>(SourceProductionContext context, TSource source);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var configFile = context.AdditionalTextsProvider
            .Where(static f => Path.GetFileName(f.Path).Equals(Configuration.FileName, StringComparison.OrdinalIgnoreCase))
            .Select(static (at, ct) => at.GetText(ct)?.ToString()).Collect().Select(static (a, _) => a.FirstOrDefault());

        var configuration = context.AnalyzerConfigOptionsProvider
                .Select(static (a, ct) => GetAnalyzerConfigValues(a.GlobalOptions))
                .Combine(context.CompilationProvider)
                .Combine(configFile)
                .Select(static (tup, ct) => Configuration.CreateConfig((CSharpCompilation)tup.Left.Right, tup.Right, tup.Left.Left));

        _linksGenerator.Initialize(context, configuration);
        _controllerGenerator.Initialize(context, configuration, context.SyntaxProvider);
        _pagesGenerator.Initialize(context, configuration, context.SyntaxProvider);
    }

    private static AnalyzerConfigValues GetAnalyzerConfigValues(AnalyzerConfigOptions analyzerConfigOptions)
    {
        if (!analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.ProjectDir, out var projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            throw new InvalidOperationException($"No AnalyzerConfigOption for {GlobalOptionConstant.BuildProperty.ProjectDir} could be found! This should not happen.");
        }

        _ = analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.RootNamespace, out var rootNamespace);

        return new AnalyzerConfigValues(projectDir, rootNamespace);
    }
}
