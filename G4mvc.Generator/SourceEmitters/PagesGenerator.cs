using G4mvc.Generator.Compilation;
using G4mvc.Generator.SourceEmitters.Base;
using System.Collections.Immutable;

namespace G4mvc.Generator.SourceEmitters;
internal class PagesGenerator : SyntaxProviderGenerator<PagesDeclarationContext>
{
#if DEBUG
    private int _version = 0;
#endif

    protected override void Execute(SourceProductionContext context, string? configFileText, ImmutableArray<PagesDeclarationContext> controllerContexts, AnalyzerConfigValues analyzerConfigValues)
    {
        throw new NotImplementedException();
    }

    protected override bool IsPossibleDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected override PagesDeclarationContext Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
