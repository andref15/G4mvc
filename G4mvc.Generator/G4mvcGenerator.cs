﻿using Microsoft.CodeAnalysis.Diagnostics;
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
        var configFile = context.AdditionalTextsProvider
            .Where(static f => Path.GetFileName(f.Path).Equals(Configuration.FileName, StringComparison.OrdinalIgnoreCase))
            .Select(static (at, ct) => at.GetText(ct)?.ToString());

        var classes = context.SyntaxProvider
            .CreateSyntaxProvider(IsPossibleControllerDeclaration, ControllerDeclarationContext.Create)
            .Where(cs => !cs.TypeSymbol.IsAbstract && cs.TypeSymbol.DerrivesFromType(TypeNames.Controller));

        IncrementalValueProvider<(ImmutableArray<ControllerDeclarationContext> ControllerContexts, ImmutableArray<string?> Configs)> configAndClasses = classes.Collect().Combine(configFile.Collect());

        var analyzerOptionsCompilationConfigAndClasses = configAndClasses
            .Combine(context.AnalyzerConfigOptionsProvider
                .Select(static (a, ct) => a.GlobalOptions))
            .Select(static (c, ct) => (Config: c.Left.Configs.FirstOrDefault(), c.Left.ControllerContexts, AnalyzerConfigOptions: c.Right));
        
        context.RegisterSourceOutput(analyzerOptionsCompilationConfigAndClasses, static (c, a) => ExecuteClassGeneration(c, a.Config, a.ControllerContexts, a.AnalyzerConfigOptions));

        IncrementalValueProvider<((AnalyzerConfigOptions AnalyzerConfigOptions, ImmutableArray<string?> ConfigFiles) Left, ParseOptions ParseOptions)> configFileAnalyzerConfigOptionsAndParseProvider = context.AnalyzerConfigOptionsProvider.Select(static (a, ct) => a.GlobalOptions).Combine(configFile.Collect()).Combine(context.ParseOptionsProvider);

        context.RegisterSourceOutput(configFileAnalyzerConfigOptionsAndParseProvider, static (c, a) => ExecuteLinksGeneration(c, a.Left.ConfigFiles.FirstOrDefault(), a.Left.AnalyzerConfigOptions, (CSharpParseOptions)a.ParseOptions));
    }

    private static bool IsPossibleControllerDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (!syntaxNode.IsKind(SyntaxKind.ClassDeclaration))
        {
            return false;
        }

        var classDeclaration = (ClassDeclarationSyntax)syntaxNode;

        return classDeclaration.Identifier.Text.EndsWith("Controller");
    }

    private static void ExecuteClassGeneration(SourceProductionContext context, string? configFileText, ImmutableArray<ControllerDeclarationContext> controllerContexts, AnalyzerConfigOptions analyzerConfigOptions)
    {
#if DEBUG
        _version++; 
#endif

        if (controllerContexts.Length is 0)
        {
            return;
        }

        var configuration = Configuration.CreateConfig((CSharpCompilation)controllerContexts[0].Model.Compilation, configFileText);

        if (!analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.ProjectDir, out var projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            return;
        }

        Dictionary<string, Dictionary<string, string>> controllerRouteClassNames = [];

        ControllerRouteClassGenerator controllerRouteClassGenerator = new(configuration);

        controllerRouteClassGenerator.AddSharedController(context, projectDir, controllerRouteClassNames);

        foreach (var controllerContextGroup in controllerContexts.GroupBy(static cc => cc.TypeSymbol.ToDisplayString()))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var controllerContextImplementations = controllerContextGroup.ToList();

            controllerRouteClassGenerator.AddControllerRouteClass(context, projectDir, controllerRouteClassNames, controllerContextImplementations);
            ControllerPartialClassGenerator.AddControllerPartialClass(context, controllerContextImplementations[0], configuration);
        }

        AreaClassesGenerator.AddAreaClasses(context, controllerRouteClassNames, configuration);

        MvcClassGenerator.AddMvcClass(context, controllerRouteClassNames, configuration
#if DEBUG
        , _version 
#endif
        );
    }

    private static void ExecuteLinksGeneration(SourceProductionContext context, string? configFileText, AnalyzerConfigOptions analyzerConfigOptions, CSharpParseOptions parseOptions)
    {
#if DEBUG
        _linksVersion++; 
#endif

        var configuration = Configuration.CreateConfig(parseOptions, configFileText);

        if (!analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.ProjectDir, out var projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            return;
        }

        LinksGenerator.AddLinksClass(context, configuration, projectDir
#if DEBUG
        , _linksVersion 
#endif
        );
    }
}
