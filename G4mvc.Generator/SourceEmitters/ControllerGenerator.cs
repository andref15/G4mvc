using G4mvc.Generator.Compilation;
using G4mvc.Generator.SourceEmitters.Base;
using System.Collections.Immutable;

namespace G4mvc.Generator.SourceEmitters;
internal class ControllerGenerator : SyntaxProviderGenerator<ControllerDeclarationContext>
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

        return classDeclaration.Identifier.Text.EndsWith(ControllerDeclarationContext.Suffix);
    }

    protected override bool DeclatationPredicate(ControllerDeclarationContext classContext)
        => !classContext.TypeSymbol.IsAbstract && classContext.TypeSymbol.DerrivesFromType(TypeNames.Controller);

    protected override ControllerDeclarationContext Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        => ControllerDeclarationContext.Create(context, cancellationToken);

    protected override void Execute(SourceProductionContext context, ImmutableArray<ControllerDeclarationContext> controllerContexts, Configuration configuration)
    {
#if DEBUG
        _version++;
#endif

        if (controllerContexts.Length is 0)
        {
            return;
        }

        var controllerRouteClassNames = new Dictionary<string, Dictionary<string, string>>();

        var controllerRouteClassGenerator = new ControllerRouteClassGenerator(configuration);

        var projectDir = configuration.AnalyzerConfigValues.ProjectDir;

        foreach (var controllerContextImplementations in controllerContexts.GroupBy(static cc => cc.TypeSymbol.ToDisplayString()).Select(g => g.ToList()))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var firstContext = controllerContextImplementations[0];

            if (firstContext.TypeSymbol.GetAttributes(true).Any(a => a.AttributeClass!.ToDisplayString() == TypeNames.NonControllerAttribute.FullName))
            {
                continue;
            }

            controllerRouteClassGenerator.AddControllerRouteClass(context, projectDir, controllerRouteClassNames, controllerContextImplementations);

            if (firstContext.DeclarationNode.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                ControllerPartialClassGenerator.AddControllerPartialClass(context, firstContext, configuration);
            }
        }

        controllerRouteClassGenerator.AddSharedControllers(context, projectDir, controllerRouteClassNames);

        AreaClassesGenerator.AddAreaClasses(context, controllerRouteClassNames, configuration);

        RouteHelperClassGenerator.AddRouteClassClass(context, configuration.JsonConfig.MvcHelperClassName, controllerRouteClassNames, configuration
#if DEBUG
        , _version
#endif
        );
    }
}
