using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;
internal class PageRouteClassGenerator(Configuration configuration)
{
    private readonly Configuration _configuration = configuration;

    public void AddSharedPages(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames)
    {
        const string sharedControllerName = "Shared";

        foreach (var (areaName, controllerRouteClasses) in controllerRouteClassNames)
        {
            if (controllerRouteClasses.ContainsKey($"{sharedControllerName}Routes"))
            {
                continue;
            }

            // TODO
        }
    }

    public void AddPageRouteClass(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> pageRouteClassNames, List<PageDeclarationContext> pageContexts)
    {
        var sourceBuilder = _configuration.CreateSourceBuilder();

        var httpMethods = pageContexts.SelectMany(c => c.DeclarationNode.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Select(md => new MethodDeclarationContext(md, c.Model, _configuration.GlobalNullable)))
            .Where(static mc => mc.MethodSymbol.DeclaredAccessibility is Accessibility.Public && RazorPageHttpMethodNames.NameSet.Contains(mc.MethodSymbol.Name)).ToList();

        var mainPageContext = pageContexts[0];

        sourceBuilder
            .Using(nameof(G4mvc))
            .AppendLine()
            .Nullable(mainPageContext.NullableEnabled);

        var pageRouteClassName = $"{mainPageContext.NameWithoutSuffix}Routes";

        AddClassNameToDictionary(pageRouteClassNames, mainPageContext.Area, mainPageContext.NameWithoutSuffix, pageRouteClassName);

        using (sourceBuilder.BeginNamespace(_configuration.GetAreaRoutesNamespace(mainPageContext.Area), true))
        using (sourceBuilder.BeginClass(_configuration.GeneratedClassModifier, pageRouteClassName))
        {
            if (mainPageContext.Area is not null)
            {
                sourceBuilder.AppendProperty("public", "string", "Area", "get", null, SourceCode.String(mainPageContext.Area));
            }

            // TODO
        }

        context.AddGeneratedSource(GetPageRoutesFileName(mainPageContext.Area, mainPageContext.NameWithoutSuffix), sourceBuilder);
    }

    private static void AddClassNameToDictionary(Dictionary<string, Dictionary<string, string>> pageRouteClassNames, string? pageArea, string pageNameWithoutSuffix, string pageRouteClassName)
    {
        var areaKey = pageArea ?? string.Empty;

        if (!pageRouteClassNames.TryGetValue(areaKey, out var classNames))
        {
            classNames = [];
            pageRouteClassNames.Add(areaKey, classNames);
        }

        classNames.Add(pageRouteClassName, pageNameWithoutSuffix);
    }

    private static string GetPageRoutesFileName(string? area, string controllerNameWithoutSuffix)
    => string.IsNullOrEmpty(area)
        ? $"{controllerNameWithoutSuffix}Routes"
        : $"{area}.{controllerNameWithoutSuffix}Routes";
}
