using G4mvc.Generator.Compilation;
using G4mvc.Generator.SourceEmitters.Base;
using System.Collections.Immutable;

namespace G4mvc.Generator.SourceEmitters;
internal class PagesGenerator : SyntaxProviderGenerator<PageDeclarationContext>
{
#if DEBUG
    private int _version = 0;
#endif

    protected override bool IsPossibleDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (!syntaxNode.IsKind(SyntaxKind.ClassDeclaration))
        {
            return false;
        }

        var classDeclaration = (ClassDeclarationSyntax)syntaxNode;

        return classDeclaration.Identifier.Text.EndsWith(PageDeclarationContext.Suffix);
    }

    protected override bool DeclatationPredicate(PageDeclarationContext classContext)
        => !classContext.TypeSymbol.IsAbstract && classContext.TypeSymbol.GetAttributes(true).Any(static a => a.AttributeClass!.ToDisplayString() == TypeNames.PageModelAttribute) && classContext.TypeSymbol.DerrivesFromType(TypeNames.PageModel);

    protected override PageDeclarationContext Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        => PageDeclarationContext.Create(context, cancellationToken);

    protected override void Execute(SourceProductionContext context, ImmutableArray<PageDeclarationContext> pageContexts, Configuration configuration)
    {
#if DEBUG
        _version++;
#endif

        if (pageContexts.Length is 0)
        {
            return;
        }

        var pageRouteClassNames = new Dictionary<string, Dictionary<string, string>>();

        var pageRouteClassGenerator = new PageRouteClassGenerator(configuration);

        var projectDir = configuration.AnalyzerConfigValues.ProjectDir;

        foreach (var pageContextImplementations in pageContexts.GroupBy(static cc => cc.TypeSymbol.ToDisplayString()).Select(g => g.ToList()))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            pageRouteClassGenerator.AddPageRouteClass(context, projectDir, pageRouteClassNames, pageContextImplementations);
        }

        AreaClassesGenerator.AddPagesAreaClasses(context, pageRouteClassNames, configuration);

        RouteHelperClassGenerator.AddPageHelperClass(context, configuration.JsonConfig.PageHelperClassName, pageRouteClassNames, configuration

#if DEBUG
        , _version
#endif
        );
    }
}
