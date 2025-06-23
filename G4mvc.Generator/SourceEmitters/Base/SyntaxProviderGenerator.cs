using G4mvc.Generator.Compilation;
using System.Collections.Immutable;

namespace G4mvc.Generator.SourceEmitters.Base;
internal abstract class SyntaxProviderGenerator<T>
    where T : ClassDeclarationContext
{
    public void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> configFile, IncrementalValueProvider<AnalyzerConfigValues> analyzerConfigOptions, SyntaxValueProvider syntaxProvider)
    {
        var classes = syntaxProvider
            .CreateSyntaxProvider(IsPossibleDeclaration, Transform)
            .Where(static cs => !cs.TypeSymbol.IsAbstract && cs.TypeSymbol.DerrivesFromType(TypeNames.Controller));

        IncrementalValueProvider<(ImmutableArray<T> ControllerContexts, string? Config)> configAndClasses = classes.Collect().Combine(configFile);

        var analyzerOptionsCompilationConfigAndClasses = configAndClasses
            .Combine(analyzerConfigOptions)
            .Select(static (c, ct) => (c.Left.Config, c.Left.ControllerContexts, AnalyzerConfigValues: c.Right));

        context.RegisterSourceOutput(analyzerOptionsCompilationConfigAndClasses, (c, a) => Execute(c, a.Config, a.ControllerContexts, a.AnalyzerConfigValues));

    }

    protected abstract bool IsPossibleDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken);
    protected abstract T Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken);
    protected abstract void Execute(SourceProductionContext context, string? configFileText, ImmutableArray<T> controllerContexts, AnalyzerConfigValues analyzerConfigValues);
}
