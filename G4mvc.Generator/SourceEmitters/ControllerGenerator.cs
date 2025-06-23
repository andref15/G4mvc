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

        return classDeclaration.Identifier.Text.EndsWith("Controller");
    }

    protected override ControllerDeclarationContext Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        => ControllerDeclarationContext.Create(context, cancellationToken);

    protected override void Execute(SourceProductionContext context, string? configFileText, ImmutableArray<ControllerDeclarationContext> controllerContexts, AnalyzerConfigValues analyzerConfigValues)
    {
        var asd = (CSharpCompilation)controllerContexts[0].Model.Compilation;

        var indexModel = asd.GetSemanticModel(asd.SyntaxTrees[0]);

        var symbol = indexModel.GetDeclaredSymbol(asd.SyntaxTrees[0].GetRoot().DescendantNodes().Where(n => n.IsKind(SyntaxKind.ClassDeclaration)).First());


#if DEBUG
        _version++;
#endif

        if (controllerContexts.Length is 0)
        {
            return;
        }

        var configuration = Configuration.CreateConfig((CSharpCompilation)controllerContexts[0].Model.Compilation, configFileText, analyzerConfigValues);

        Dictionary<string, Dictionary<string, string>> controllerRouteClassNames = [];

        ControllerRouteClassGenerator controllerRouteClassGenerator = new(configuration);

        controllerRouteClassGenerator.AddSharedController(context, analyzerConfigValues.ProjectDir, controllerRouteClassNames);

        foreach (var controllerContextGroup in controllerContexts.GroupBy(static cc => cc.TypeSymbol.ToDisplayString()))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var controllerContextImplementations = controllerContextGroup.ToList();

            controllerRouteClassGenerator.AddControllerRouteClass(context, analyzerConfigValues.ProjectDir, controllerRouteClassNames, controllerContextImplementations);
            ControllerPartialClassGenerator.AddControllerPartialClass(context, controllerContextImplementations[0], configuration);
        }

        AreaClassesGenerator.AddAreaClasses(context, controllerRouteClassNames, configuration);

        MvcClassGenerator.AddMvcClass(context, controllerRouteClassNames, configuration
#if DEBUG
        , _version
#endif
        );
    }
}
