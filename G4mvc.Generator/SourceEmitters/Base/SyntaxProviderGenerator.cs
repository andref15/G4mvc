using G4mvc.Generator.Compilation;
using System.Collections.Immutable;

namespace G4mvc.Generator.SourceEmitters.Base;
internal abstract class SyntaxProviderGenerator<T>
    where T : ClassDeclarationContext
{
    public void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<Configuration> configurationProvider, SyntaxValueProvider syntaxProvider)
    {
        var classes = syntaxProvider
            .CreateSyntaxProvider(IsPossibleDeclaration, Transform)
            .Where(DeclatationPredicate);

        var configAndClasses = classes.Collect().Combine(configurationProvider);

        context.RegisterSourceOutput(configAndClasses, (c, a) => Execute(c, a.Left, a.Right));

    }

    protected abstract bool IsPossibleDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken);
    protected abstract bool DeclatationPredicate(T classContext);
    protected abstract T Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken);
    protected abstract void Execute(SourceProductionContext context, ImmutableArray<T> classContexts, Configuration configuration);
}
