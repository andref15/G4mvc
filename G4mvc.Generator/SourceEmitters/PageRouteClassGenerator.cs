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
            .Where(static mc => mc.MethodSymbol.DeclaredAccessibility is Accessibility.Public && RazorPageHttpMethodNames.IsMatch(mc.MethodSymbol.Name) && mc.MethodSymbol.GetAttributes().Any(a => a.AttributeClass!.ToDisplayString() == TypeNames.NonHandlerAttribute.FullName)).ToList();

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

            sourceBuilder
                .AppendProperty("public", "string", "Name", "get", null, SourceCode.String(mainPageContext.Area))
                .AppendProperty("public", $"{mainPageContext.NameWithoutSuffix}HttpMethods", "HttpMethods", "get", null, SourceCode.String(mainPageContext.Area))
                .AppendProperty("public", $"{mainPageContext.NameWithoutSuffix}View", "View", "get", null, SourceCode.NewCtor);

            var httpMethodGroups = httpMethods.GroupBy(static hm => hm.Syntax.Identifier.Text.RemoveEnd("Async")).ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var actionName in httpMethodGroups.Keys)
            {
                sourceBuilder.AppendProperty("public", $"{actionName}ParamsClass", $"{actionName}Params", "get", null, SourceCode.NewCtor);
            }

            sourceBuilder.AppendLine();

            var methodParameterGroups = AddHttpMethodsAndGetParameterGroups(context, mainPageContext, sourceBuilder, httpMethods);
        }

        context.AddGeneratedSource(GetPageRoutesFileName(mainPageContext.Area, mainPageContext.NameWithoutSuffix), sourceBuilder);
    }

    private static string? GetDefaultValue(ParameterSyntax syntax)
        => syntax.Default is null ? null : $" {syntax.Default}";

    private static Dictionary<string, HashSet<string>> AddHttpMethodsAndGetParameterGroups(SourceProductionContext context, PageDeclarationContext mainPageContext, SourceBuilder sourceBuilder, Dictionary<string, IEnumerable<MethodDeclarationContext>> httpMethodGroups)
    {
        var methodParameterGroups = new Dictionary<string, HashSet<string>>();

        foreach (var (methodName, httpMethodsGroup) in httpMethodGroups)
        {
            context.CancellationToken.ThrowIfCancellationRequested();


            var methodsGroupParameterNames = new HashSet<string>();
            methodParameterGroups.Add(methodName, methodsGroupParameterNames);

            using (sourceBuilder.BeginMethod("public", nameof(G4mvcPageRouteValues), methodName))
            {
                sourceBuilder.AppendReturnCtor(nameof(G4mvcPageRouteValues), SourceCode.String(mainPageContext.Area), SourceCode.String(mainPageContext.NameWithoutSuffix), SourceCode.String(methodName));
            }

            sourceBuilder.AppendLine();

            foreach (var httpMethodContext in httpMethodsGroup.Where(md => md.Syntax.ParameterList.Parameters.Count > 0))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                var relevantParameters = httpMethodContext.Syntax.ParameterList.Parameters.Select(p => new ParameterContext(p, httpMethodContext.Model.GetDeclaredSymbol(p)!)).Where(p => p.Symbol.Type.ToDisplayString() != TypeNames.CancellationToken).ToList();

                if (relevantParameters.Count is 0)
                {
                    continue;
                }

                foreach (var paramName in relevantParameters.Select(p => p.Symbol.Name))
                {
                    methodsGroupParameterNames.Add(paramName);
                }

                var nullableBlock = (SourceBuilder.NullableBlock?)null;

                if (mainPageContext.NullableEnabled != httpMethodContext.NullableEnabled)
                {
                    nullableBlock = sourceBuilder.BeginNullable(httpMethodContext.NullableEnabled);
                }

                using (nullableBlock)
                using (sourceBuilder.BeginMethod("public", nameof(G4mvcActionRouteValues), methodName, string.Join(", ", relevantParameters.Select(p => $"{p.Symbol.Type} {p.Symbol.Name}{GetDefaultValue(p.Syntax)}"))))
                {
                    sourceBuilder.AppendLine($"var route = {methodName}()").AppendLine();

                    foreach (var parameter in relevantParameters)
                    {
                        context.CancellationToken.ThrowIfCancellationRequested();

                        sourceBuilder.AppendLine($"route[{SourceCode.String(parameter.Symbol.Name)}] = {parameter.Symbol.Name}");
                    }

                    sourceBuilder.AppendLine().AppendLine("return route");
                }
            }
        }

        return methodParameterGroups;
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
