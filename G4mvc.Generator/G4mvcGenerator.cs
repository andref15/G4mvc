using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace G4mvc.Generator;

[Generator(LanguageNames.CSharp)]
public class G4mvcGenerator : IIncrementalGenerator
{
#if DEBUG
    private static int _version = 0;
    private static int _linksVersion = 0;
#endif

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<string?> configFile = context.AdditionalTextsProvider
            .Where(static f => Path.GetFileName(f.Path).Equals(Configuration.FileName, StringComparison.OrdinalIgnoreCase))
            .Select(static (at, ct) => at.GetText(ct)?.ToString());

        IncrementalValuesProvider<ControllerDeclarationContext> classes = context.SyntaxProvider
            .CreateSyntaxProvider(IsPossibleControllerDeclaration, ControllerDeclarationContext.Create)
            .Where(cs => !cs.TypeSymbol.IsAbstract && cs.TypeSymbol.DerrivesFromType(TypeNames.Controller));

        IncrementalValuesProvider<(string? Config, ImmutableArray<ControllerDeclarationContext> ControllerContexts)> configAndClasses = configFile.Combine(classes.Collect());

        IncrementalValuesProvider<(string? Config, ImmutableArray<ControllerDeclarationContext> ControllerContexts, AnalyzerConfigOptions AnalyzerConfigOptions)> analyzerOptionsCompilationConfigAndClasses = configAndClasses
            .Combine(context.AnalyzerConfigOptionsProvider
                .Select(static (a, ct) => a.GlobalOptions))
            .Select(static (c, ct) => (c.Left.Config, c.Left.ControllerContexts, AnalyzerConfigOptions: c.Right));

        context.RegisterSourceOutput(analyzerOptionsCompilationConfigAndClasses, static (c, a) => ExecuteClassGeneration(c, a.Config, a.ControllerContexts, a.AnalyzerConfigOptions));

        context.RegisterSourceOutput(configFile.Combine(context.AnalyzerConfigOptionsProvider.Select(static (a, ct) => a.GlobalOptions)), static (c, a) => ExecuteLinksGeneration(c, a.Right));
    }

    private static bool IsPossibleControllerDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (!syntaxNode.IsKind(SyntaxKind.ClassDeclaration))
        {
            return false;
        }

        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)syntaxNode;

        return classDeclaration.BaseList?.Types.Any() ?? false;
    }

    private static void ExecuteClassGeneration(SourceProductionContext context, string? additionalFileText, ImmutableArray<ControllerDeclarationContext> controllerContexts, AnalyzerConfigOptions analyzerConfigOptions)
    {
#if DEBUG
        _version++; 
#endif

        if (controllerContexts.Length is 0)
        {
            return;
        }

        Configuration.CreateConfig((CSharpCompilation)controllerContexts[0].Model.Compilation, additionalFileText);

        if (!analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.ProjectDir, out string? projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            return;
        }

        Dictionary<string, Dictionary<string, string>> controllerRouteClassNames = new();

        ControllerRouteClassGenerator.AddSharedController(context, projectDir, controllerRouteClassNames);

        foreach (ControllerDeclarationContext controllerContext in controllerContexts)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            ControllerRouteClassGenerator.AddControllerRouteClass(context, projectDir, controllerRouteClassNames, controllerContext);
            ControllerPartialClassGenerator.AddControllerPartialClass(context, controllerContext);
        }

        AreaClassesGenerator.AddAreaClasses(context, controllerRouteClassNames);

        MvcClassGenerator.AddMvcClass(context, controllerRouteClassNames
#if DEBUG
        , _version 
#endif
        );
    }

    private static void ExecuteLinksGeneration(SourceProductionContext context, AnalyzerConfigOptions analyzerConfigOptions)
    {
#if DEBUG
        _linksVersion++; 
#endif

        if (!analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.ProjectDir, out string? projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            return;
        }

        LinksGenerator.AddLinksClass(context, projectDir
#if DEBUG
        , _linksVersion 
#endif
        );
    }
}
