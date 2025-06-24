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
        => !classContext.TypeSymbol.IsAbstract && classContext.TypeSymbol.DerrivesFromType(TypeNames.PageModel);

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

        var controllerRouteClassNames = new Dictionary<string, Dictionary<string, string>>();

        var controllerRouteClassGenerator = new ControllerRouteClassGenerator(configuration);

        var projectDir = configuration.AnalyzerConfigValues.ProjectDir;

        controllerRouteClassGenerator.AddSharedController(context, projectDir, controllerRouteClassNames);

        foreach (var controllerContextGroup in pageContexts.GroupBy(static cc => cc.TypeSymbol.ToDisplayString()))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var controllerContextImplementations = controllerContextGroup.ToList();
            controllerRouteClassGenerator.AddControllerRouteClass(context, projectDir, controllerRouteClassNames, controllerContextImplementations);

        }

        AreaClassesGenerator.AddAreaClasses(context, controllerRouteClassNames, configuration);

        RouteHelperClassGenerator.AddRouteClassClass(context, configuration.JsonConfig.PageHelperClassName, controllerRouteClassNames, configuration

#if DEBUG
        , _version
#endif
        );
    }
}
