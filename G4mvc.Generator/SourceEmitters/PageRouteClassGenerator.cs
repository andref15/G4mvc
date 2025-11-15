using G4mvc.Generator.Compilation;
using System.Text;

namespace G4mvc.Generator.SourceEmitters;

internal class PageRouteClassGenerator(Configuration configuration)
{
    private readonly Configuration _configuration = configuration;

    public void AddPageRouteClass(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> pageRouteClassNames, List<PageDeclarationContext> pageContexts)
    {
        var sourceBuilder = _configuration.CreateSourceBuilder();

        var httpMethods = pageContexts.SelectMany(c => c.DeclarationNode.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Select(md => new MethodDeclarationContext(md, c.Model, _configuration.GlobalNullable)))
            .Where(static mc => mc.MethodSymbol.DeclaredAccessibility is Accessibility.Public && RazorPageHttpMethodNames.IsMatch(mc.MethodSymbol.Name) && !mc.MethodSymbol.GetAttributes().Any(a => a.AttributeClass!.ToDisplayString() == TypeNames.NonHandlerAttribute.FullName)).ToList();
        var bindModelProperties = pageContexts.SelectMany(c => c.DeclarationNode.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                .Select(pd => (Pd: pd, Symbol: c.Model.GetDeclaredSymbol(pd)!)).Where(static pds => pds.Symbol.GetAttributes().Any(a => a.AttributeClass!.ToDisplayString() == TypeNames.BindPropertyAttribute.FullName))
            .Select(pds => new ModelBindingPropertyDeclarationContext(pds.Pd, c.Model, pds.Symbol, _configuration.GlobalNullable))).ToList();

        var mainPageContext = pageContexts[0];

        sourceBuilder
            .Using(nameof(G4mvc))
            .AppendLine()
            .Nullable(mainPageContext.NullableEnabled);

        var topClassName = (string?)null;
        var topClassNameWithoutSuffix = (string?)null;
        var pageRouteClassName = $"{mainPageContext.NameWithoutSuffix}Routes";

        using (sourceBuilder.BeginNamespace(_configuration.GetPagesNamespace(mainPageContext.Area), true))
        using (BeginContainingNamespacesAsClasses(sourceBuilder, mainPageContext.TypeSymbol, _configuration.GeneratedClassModifier, ref topClassName, ref topClassNameWithoutSuffix))
        using (sourceBuilder.BeginClass($"{_configuration.GeneratedClassModifier} partial", pageRouteClassName))
        {
            if (topClassName is null || topClassNameWithoutSuffix is null)
            {
                topClassName = pageRouteClassName;
                topClassNameWithoutSuffix = mainPageContext.NameWithoutSuffix;
            }

            AddClassNameToDictionary(pageRouteClassNames, mainPageContext.Area, topClassNameWithoutSuffix, topClassName);

            if (mainPageContext.Area is not null)
            {
                sourceBuilder.AppendProperty("public", "string", "Area", "get", null, SourceCode.String(mainPageContext.Area));
            }

            sourceBuilder
                .AppendProperty("public", "string", "Name", "get", null, SourceCode.String(mainPageContext.NameWithoutSuffix))
                .AppendProperty("public", $"{mainPageContext.NameWithoutSuffix}MethodNames", "HttpMethods", "get", null, SourceCode.NewCtor)
                .AppendProperty("public", $"{mainPageContext.NameWithoutSuffix}View", "View", "get", null, SourceCode.NewCtor);

            var httpMethodGroups = httpMethods.GroupBy(static hm => hm.Syntax.Identifier.Text.Remove("On", "Async", StringComparison.OrdinalIgnoreCase)).ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var actionName in httpMethodGroups.Keys)
            {
                sourceBuilder.AppendProperty("public", $"{actionName}ParamsClass", $"{actionName}Params", "get", null, SourceCode.NewCtor);
            }

            sourceBuilder.AppendLine();

            var methodParameterGroups = AddHttpMethodsAndGetParameterGroups(context, mainPageContext, sourceBuilder, httpMethodGroups, bindModelProperties);

            using (sourceBuilder.BeginClass("public", $"{mainPageContext.NameWithoutSuffix}MethodNames"))
            {
                foreach (var actionName in httpMethodGroups.Keys)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    sourceBuilder.AppendProperty("public", "string", actionName, "get", null, SourceCode.Nameof(actionName));
                }
            }

            foreach (var (methodName, paramNames) in methodParameterGroups)
            {
                sourceBuilder.AppendLine();

                using (sourceBuilder.BeginClass("public", $"{methodName}ParamsClass"))
                {
                    foreach (var paramName in paramNames)
                    {
                        sourceBuilder.AppendProperty("public", "string", paramName, $"get", null, SourceCode.Nameof(paramName));
                    }
                }
            }

            var viewFile = new FileInfo(mainPageContext.DeclarationNode.SyntaxTree.FilePath.RemoveEnd(".cs", StringComparison.Ordinal));

            if (viewFile.Exists)
            {
                using (sourceBuilder.BeginClass("public", $"{mainPageContext.NameWithoutSuffix}View"))
                {
                    var appRootPrefix = projectDir[projectDir.Length - 1] == Path.DirectorySeparatorChar ? "~/" : "~";

                    sourceBuilder.AppendProperty("public", "string", "Name", "get", null, SourceCode.String(Path.GetFileNameWithoutExtension(viewFile.Name)));
                    sourceBuilder.AppendProperty("public", "string", "AppPath", "get", null, SourceCode.String(viewFile.FullName.Replace(projectDir, appRootPrefix).Replace("\\", "/")));
                }
            }
        }

        context.AddGeneratedSource(GetPageRoutesFileName(mainPageContext), sourceBuilder);
    }

    private static string? GetDefaultValue(ParameterSyntax syntax)
        => syntax.Default is null ? null : $" {syntax.Default}";

    private static IDisposable? BeginContainingNamespacesAsClasses(SourceBuilder sourceBuilder, INamespaceOrTypeSymbol namespaceOrType, string generatedClassModifier, ref string? topNamespaceClassName, ref string? topNamespaceClassNameWithoutSuffix)
    {
        var containingNamespace = namespaceOrType.ContainingNamespace;
        if (containingNamespace.Name is { Length: 0 } or "Pages")
        {
            return null;
        }

        topNamespaceClassNameWithoutSuffix = containingNamespace.Name;
        var namespaceClassName = $"{topNamespaceClassNameWithoutSuffix}Routes";
        topNamespaceClassName = namespaceClassName;

        var containingClass = BeginContainingNamespacesAsClasses(sourceBuilder, containingNamespace, generatedClassModifier, ref topNamespaceClassName, ref topNamespaceClassNameWithoutSuffix);
        var thisClass = sourceBuilder.BeginClass($"{generatedClassModifier} partial", namespaceClassName);

        return containingClass is null ? thisClass : JoinedDisposable.Create(containingClass, thisClass);
    }

    private static Dictionary<string, HashSet<string>> AddHttpMethodsAndGetParameterGroups(SourceProductionContext context, PageDeclarationContext mainPageContext, SourceBuilder sourceBuilder, Dictionary<string, IEnumerable<MethodDeclarationContext>> httpMethodGroups, List<ModelBindingPropertyDeclarationContext> bindModelProperties)
    {
        var methodParameterGroups = new Dictionary<string, HashSet<string>>();

        var bindModelPropertyParameters = new List<(string Type, string Name, string? DefaultAssignment)>();
        var bindModelPropertyParametersSupportGet = new List<(string Type, string Name, string? DefaultAssignment)>();
        foreach (var bindModelProperty in bindModelProperties)
        {
            var type = $"global::{bindModelProperty.PropertySymbol.Type.ToDisplayString()}";
            var name = bindModelProperty.PropertySymbol.Name.FirstCharLower();

            if ((bool?)bindModelProperty.BindPropertyAttribute.NamedArguments.FirstOrDefault(arg => arg.Key == TypeNames.BindPropertyAttribute.NamedArguments.SupportsGet).Value.Value ?? false)
            {
                bindModelPropertyParametersSupportGet.Add((type, name, null));
            }

            bindModelPropertyParameters.Add((type, name, null));
        }

        var namespacePagePath = GetContainingNamespacesAsPagePath(mainPageContext.TypeSymbol);
        var areaString = SourceCode.String(mainPageContext.Area);
        var pageString = SourceCode.String($"{namespacePagePath}{mainPageContext.NameWithoutSuffix}");
        foreach (var (handlerHethodName, httpMethodsGroup) in httpMethodGroups)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var methodsGroupParameterNames = new HashSet<string>();
            methodParameterGroups.Add(handlerHethodName, methodsGroupParameterNames);

            var (method, handlerName) = RazorPageHttpMethodNames.ParseMethodAndHandlerName(handlerHethodName);
            using (sourceBuilder.BeginMethod("public", nameof(G4mvcPageRouteValues), handlerHethodName))
            {
                sourceBuilder.AppendReturnCtor(nameof(G4mvcPageRouteValues), areaString, pageString, SourceCode.String(handlerName), SourceCode.String(method));
            }

            sourceBuilder.AppendLine();

            foreach (var httpMethodContext in httpMethodsGroup.Where(md => md.Syntax.ParameterList.Parameters.Count > 0))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                var relevantParameterEnumerable = httpMethodContext.Syntax.ParameterList.Parameters
                    .Select(p => new ParameterContext(p, httpMethodContext.Model.GetDeclaredSymbol(p)!))
                    .Where(p => p.Symbol.Type.ToDisplayString() != TypeNames.CancellationToken)
                    .Select(p => (Type: p.Symbol.Type.ToDisplayString(), p.Symbol.Name, DefaultAssignment: GetDefaultValue(p.Syntax)));

                if (method == RazorPageHttpMethodNames.Get)
                {
                    relevantParameterEnumerable = relevantParameterEnumerable.Union(bindModelPropertyParametersSupportGet);
                }
                else
                {
                    relevantParameterEnumerable = relevantParameterEnumerable.Union(bindModelPropertyParameters);
                }

                var relevantParameters = relevantParameterEnumerable.OrderBy(p => p.DefaultAssignment is not null).ToList();

                if (relevantParameters.Count is 0)
                {
                    continue;
                }

                foreach (var paramName in relevantParameters.Select(p => p.Name))
                {
                    methodsGroupParameterNames.Add(paramName);
                }

                var nullableBlock = (SourceBuilder.NullableBlock?)null;

                if (mainPageContext.NullableEnabled != httpMethodContext.NullableEnabled)
                {
                    nullableBlock = sourceBuilder.BeginNullable(httpMethodContext.NullableEnabled);
                }

                using (nullableBlock)
                using (sourceBuilder.BeginMethod("public", nameof(G4mvcPageRouteValues), handlerHethodName, string.Join(", ", relevantParameters.Select(p => $"{p.Type} {p.Name}{p.DefaultAssignment}"))))
                {
                    sourceBuilder.AppendLine($"var route = {handlerHethodName}()").AppendLine();

                    foreach (var parameter in relevantParameters)
                    {
                        context.CancellationToken.ThrowIfCancellationRequested();

                        sourceBuilder.AppendLine($"route[{SourceCode.String(parameter.Name)}] = {parameter.Name}");
                    }

                    sourceBuilder.AppendLine().AppendLine("return route");
                }
            }
        }

        return methodParameterGroups;
    }

    private static string GetContainingNamespacesAsPagePath(ITypeSymbol typeSymbol)
    {
        var sb = new StringBuilder("/");
        Internal(sb, typeSymbol.ContainingNamespace);

        return sb.ToString();

        static void Internal(StringBuilder stringBuilder, INamespaceSymbol @namespace)
        {
            if (@namespace.Name is { Length: 0 } or "Pages")
            {
                return;
            }

            Internal(stringBuilder, @namespace.ContainingNamespace);
            stringBuilder.Append(@namespace.Name).Append('/');
        }
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

    private static string GetPageRoutesFileName(PageDeclarationContext pageDeclarationContext)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(pageDeclarationContext.Area))
        {
            sb.Append(pageDeclarationContext.Area).Append('.');
        }

        IterateNamespacesInReverse(sb, pageDeclarationContext.TypeSymbol.ContainingNamespace);

        sb.Append(pageDeclarationContext.NameWithoutSuffix).Append("Routes");

        return sb.ToString();

        static void IterateNamespacesInReverse(StringBuilder stringBuilder, INamespaceSymbol @namespace)
        {
            if (@namespace.Name is { Length: 0 } or "Pages")
            {
                return;
            }

            IterateNamespacesInReverse(stringBuilder, @namespace.ContainingNamespace);
            stringBuilder.Append(@namespace.Name).Append('.');
        }
    }
}
