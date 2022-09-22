using System.Diagnostics;

namespace G4mvc.Generator;

[Generator]
public class G4mvcGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
//#if DEBUG
//        if (!Debugger.IsAttached)
//        {
//            Debugger.Launch();
//        }
//#endif
    }

    public void Execute(GeneratorExecutionContext context)
    {
        Configuration.CreateConfig(context);

        if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out string? projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            return;
        }

        Dictionary<string, Dictionary<string, string>> controllerRouteClassNames = new();

        ControllerRouteClassGenerator.AddSharedController(context, projectDir, controllerRouteClassNames);

        IEnumerable<ControllerDeclarationContext> controllerContexts = context.Compilation.SyntaxTrees
            .SelectMany(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            .Select(ct => new ControllerDeclarationContext(ct, context.Compilation.GetSemanticModel(ct.SyntaxTree)))
            .Where(cs => !cs.TypeSymbol.IsAbstract && cs.TypeSymbol.DerrivesFromType(TypeNames.Controller));

        foreach (ControllerDeclarationContext controllerContext in controllerContexts)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            ControllerRouteClassGenerator.AddControllerRouteClass(context, projectDir, controllerRouteClassNames, controllerContext);
            ControllerPartialClassGenerator.AddControllerPartialClass(context, controllerContext);
        }

        AreaClassesGenerator.AddAreaClasses(context, controllerRouteClassNames);

        MvcClassGenerator.AddMvcClass(context, controllerRouteClassNames);

        LinksGenerator.AddLinksClass(context, projectDir);

    }
}
