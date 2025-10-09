using G4mvc.Generator.Compilation;
using G4mvc.Generator.SourceEmitters;

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
                .Select(static (a, ct) => AnalyzerConfigValues.FromAnalyzerConfigOptions(a.GlobalOptions))
                .Combine(context.CompilationProvider)
                .Combine(configFile)
                .Select(static (tup, ct) => Configuration.CreateConfig((CSharpCompilation)tup.Left.Right, tup.Right, tup.Left.Left));

        var views = context.AdditionalTextsProvider
            .Where(static f => Path.GetExtension(f.Path).Equals(".cshtml", StringComparison.OrdinalIgnoreCase));

        _linksGenerator.Initialize(context, configuration);
        _controllerGenerator.Initialize(context, configuration, context.SyntaxProvider, views);
        _pagesGenerator.Initialize(context, configuration, context.SyntaxProvider, views);
    }
}
