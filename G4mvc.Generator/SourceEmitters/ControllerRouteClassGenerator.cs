using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;
internal class ControllerRouteClassGenerator(Configuration configuration)
{
    private readonly Configuration _configuration = configuration;

    internal void AddSharedControllers(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames)
    {
        const string sharedControllerName = "Shared";

        foreach (var (areaName, controllerRouteClasses) in controllerRouteClassNames)
        {
            if (controllerRouteClasses.ContainsKey($"{sharedControllerName}Routes"))
            {
                continue;
            }

            AddClassNameToDictionary(controllerRouteClassNames, areaName, sharedControllerName, $"{sharedControllerName}Routes");

            AddViewsOnlyRoutesClass(context, projectDir, areaName, sharedControllerName);
        }
    }

    internal void AddControllerRouteClass(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, List<ControllerDeclarationContext> controllerContexts)
    {
        var sourceBuilder = _configuration.CreateSourceBuilder();

        var httpMethods = controllerContexts.SelectMany(c => c.DeclarationNode.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    .Select(md => new MethodDeclarationContext(md, c.Model, _configuration.GlobalNullable)))
                .Where(static (mc) => !mc.MethodSymbol.GetAttributes().Any(a => a.AttributeClass!.ToDisplayString() == TypeNames.NonActionAttribute.FullName) && IsActionResult(mc.MethodSymbol.ReturnType)).ToList();

        var mainControllerContext = controllerContexts[0];

        sourceBuilder
            .Using(nameof(G4mvc))
            .AppendLine()
            .Nullable(controllerContexts[0].NullableEnabled);

        var controllerRouteClassName = $"{mainControllerContext.NameWithoutSuffix}Routes";
        AddClassNameToDictionary(controllerRouteClassNames, mainControllerContext.Area, mainControllerContext.NameWithoutSuffix, controllerRouteClassName);

        using (sourceBuilder.BeginNamespace(_configuration.GetControllerRoutesNamespace(mainControllerContext.Area), true))
        using (sourceBuilder.BeginClass(_configuration.GeneratedClassModifier, controllerRouteClassName))
        {
            if (mainControllerContext.Area is not null)
            {
                sourceBuilder.AppendProperty("public", "string", "Area", "get", null, SourceCode.String(mainControllerContext.Area));
            }

            sourceBuilder
                .AppendProperty("public", "string", "Name", "get", null, SourceCode.String(mainControllerContext.NameWithoutSuffix))
                .AppendProperty("public", $"{mainControllerContext.NameWithoutSuffix}ActionNames", "ActionNames", "get", null, SourceCode.NewCtor)
                .AppendProperty("public", $"{mainControllerContext.NameWithoutSuffix}Views", "Views", "get", null, SourceCode.NewCtor);

            var httpMethodGroups = httpMethods.GroupBy(md => md.Syntax.Identifier.Text.RemoveEnd("Async")).ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var actionName in httpMethodGroups.Keys)
            {
                sourceBuilder.AppendProperty("public", $"{actionName}ParamsClass", $"{actionName}Params", "get", null, SourceCode.NewCtor);
            }

            sourceBuilder.AppendLine();

            var actionParameterGroups = AddActionMethodsAndGetParameterGroups(context, mainControllerContext, sourceBuilder, httpMethods);

            using (sourceBuilder.BeginClass("public", $"{mainControllerContext.NameWithoutSuffix}ActionNames"))
            {
                foreach (var actionName in httpMethodGroups.Keys)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    sourceBuilder.AppendProperty("public", "string", actionName, "get", null, SourceCode.Nameof(actionName));
                }
            }

            foreach (var actionParameters in actionParameterGroups)
            {
                sourceBuilder.AppendLine();

                using (sourceBuilder.BeginClass("public", $"{actionParameters.Key}ParamsClass"))
                {
                    foreach (var paramName in actionParameters.Value)
                    {
                        sourceBuilder.AppendProperty("public", "string", paramName, $"get", null, SourceCode.Nameof(paramName));
                    }
                }
            }

            sourceBuilder.AppendLine();

            var viewsDirectory = GetViewsDirectoryForController(projectDir, mainControllerContext);

            AddViewsClass(sourceBuilder, projectDir, viewsDirectory, mainControllerContext.NameWithoutSuffix, _configuration.JsonConfig.EnableSubfoldersInViews);
        }

        context.AddGeneratedSource(GetControllerRoutesFileName(mainControllerContext.Area, mainControllerContext.NameWithoutSuffix), sourceBuilder);
    }

    private void AddViewsOnlyRoutesClass(SourceProductionContext context, string projectDir, string? areaName, string controllerNameWithoutSuffix)
    {
        var sourceBuilder = _configuration.CreateSourceBuilder();

        sourceBuilder.Nullable(_configuration.GlobalNullable);

        using (sourceBuilder.BeginNamespace(_configuration.GetControllerRoutesNamespace(areaName), true))
        using (sourceBuilder.BeginClass(_configuration.GeneratedClassModifier, $"{controllerNameWithoutSuffix}Routes"))
        {
            var directory = new DirectoryInfo(Path.Combine(projectDir, areaName.IfNotNullNullOrEmpty("Areas"), areaName, "Views", controllerNameWithoutSuffix));

            if (directory.Exists)
            {
                sourceBuilder.AppendProperty("public", $"{controllerNameWithoutSuffix}Views", "Views", "get", null, SourceCode.NewCtor);
                AddViewsClass(sourceBuilder, projectDir, directory, controllerNameWithoutSuffix, _configuration.JsonConfig.EnableSubfoldersInViews);
            }
        }

        context.AddGeneratedSource(GetControllerRoutesFileName(areaName, controllerNameWithoutSuffix), sourceBuilder);
    }

    private static Dictionary<string, HashSet<string>> AddActionMethodsAndGetParameterGroups(SourceProductionContext context, ControllerDeclarationContext mainControllerContext, SourceBuilder sourceBuilder, List<MethodDeclarationContext> httpMethods)
    {
        var actionParameterGroups = new Dictionary<string, HashSet<string>>();

        foreach (var httpMethodsGroup in httpMethods.GroupBy(md => md.Syntax.Identifier.Text.RemoveEnd("Async")))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var actionName = httpMethodsGroup.Key;

            var methodsGroupParameterNames = new HashSet<string>();
            actionParameterGroups.Add(actionName, methodsGroupParameterNames);

            using (sourceBuilder.BeginMethod("public", nameof(G4mvcActionRouteValues), actionName))
            {
                sourceBuilder.AppendReturnCtor(nameof(G4mvcActionRouteValues), SourceCode.String(mainControllerContext.Area), SourceCode.String(mainControllerContext.NameWithoutSuffix), SourceCode.String(actionName));
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

                if (mainControllerContext.NullableEnabled != httpMethodContext.NullableEnabled)
                {
                    nullableBlock = sourceBuilder.BeginNullable(httpMethodContext.NullableEnabled);
                }

                using (nullableBlock)
                using (sourceBuilder.BeginMethod("public", nameof(G4mvcActionRouteValues), actionName, string.Join(", ", relevantParameters.Select(p => $"{p.Symbol.Type} {p.Symbol.Name}{GetDefaultValue(p.Syntax)}"))))
                {
                    sourceBuilder.AppendLine($"{nameof(G4mvcActionRouteValues)} route = {actionName}()").AppendLine();

                    foreach (var parameter in relevantParameters)
                    {
                        context.CancellationToken.ThrowIfCancellationRequested();

                        sourceBuilder.AppendLine($"route[{SourceCode.String(parameter.Symbol.Name)}] = {parameter.Symbol.Name}");
                    }

                    sourceBuilder.AppendLine().AppendLine("return route");
                }

                sourceBuilder.AppendLine();
            }
        }

        return actionParameterGroups;
    }

    private static DirectoryInfo GetViewsDirectoryForController(string projectDir, ControllerDeclarationContext mainControllerContext)
    {
        var root = mainControllerContext.Area is null ? Path.Combine(projectDir, "Views", mainControllerContext.NameWithoutSuffix) : Path.Combine(projectDir, "Areas", mainControllerContext.Area, "Views", mainControllerContext.NameWithoutSuffix);
        var directory = new DirectoryInfo(root);
        return directory;
    }

    private static string? GetDefaultValue(ParameterSyntax syntax)
        => syntax.Default is null ? null : $" {syntax.Default}";

    private static bool IsActionResult(ITypeSymbol returnType)
        => IsOrImplementsActionResultIInterfaces(returnType) || (returnType is INamedTypeSymbol namedReturnType && namedReturnType.DerrivesFromType(TypeNames.Task) && namedReturnType.IsGenericType && IsOrImplementsActionResultIInterfaces(namedReturnType.TypeArguments[0]));

    private static bool IsOrImplementsActionResultIInterfaces(ITypeSymbol type)
        => type.IsOrImplementsInterface(TypeNames.IActionResult) || type.IsOrImplementsInterface(TypeNames.IConvertToActionResult);

    private static void AddClassNameToDictionary(Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, string? controllerArea, string controllerNameWithoutSuffix, string controllerRouteClassName)
    {
        if (!controllerRouteClassNames.ContainsKey(controllerArea ?? string.Empty))
        {
            controllerRouteClassNames[controllerArea ?? string.Empty] = [];
        }

        controllerRouteClassNames[controllerArea ?? string.Empty].Add(controllerRouteClassName, controllerNameWithoutSuffix);
    }

    private static void AddViewsClass(SourceBuilder sourceBuilder, string projectDir, DirectoryInfo directoryInfo, string className, bool enumerateSubDirectories)
    {
        using (sourceBuilder.BeginClass("public", $"{className}Views"))
        {
            if (!directoryInfo.Exists)
            {
                return;
            }

            sourceBuilder.AppendProperty("public", $"{className}ViewNames", "ViewNames", "get", null, SourceCode.NewCtor);

            List<string> viewNames = [];
            foreach (var view in GetViewsForController(projectDir, directoryInfo))
            {
                viewNames.Add(view.Key);
                sourceBuilder.AppendProperty("public", "string", view.Key, "get", null, SourceCode.String(view.Value));
            }

            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginClass("public", $"{className}ViewNames"))
            {
                foreach (var viewName in viewNames)
                {
                    sourceBuilder.AppendProperty("public", "string", viewName, "get", null, SourceCode.Nameof(viewName));
                }
            }

            if (enumerateSubDirectories)
            {
                var classNameSpan = className.AsSpan();

                foreach (var subDir in directoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).OrderBy(d => d.Name))
                {
                    var subClassName = IdentifierParser.CreateIdentifierFromPath(subDir.Name, classNameSpan);

                    sourceBuilder.AppendProperty("public", $"{subClassName}Views", subClassName, "get", null, SourceCode.NewCtor);
                    AddViewsClass(sourceBuilder, projectDir, subDir, subClassName, enumerateSubDirectories);
                }
            }
        }
    }

    private static IEnumerable<KeyValuePair<string, string>> GetViewsForController(string projectDir, DirectoryInfo directoryInfo)
    {
        var appRootPrefix = projectDir[projectDir.Length - 1] == Path.DirectorySeparatorChar ? "~/" : "~";

        foreach (var file in directoryInfo.EnumerateFiles("*.cshtml").OrderBy(f => f.Name))
        {
            yield return new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(file.Name), file.FullName.Replace(projectDir, appRootPrefix).Replace("\\", "/"));
        }
    }

    private static string GetControllerRoutesFileName(string? area, string controllerNameWithoutSuffix)
        => string.IsNullOrEmpty(area)
            ? $"{controllerNameWithoutSuffix}Routes"
            : $"{area}.{controllerNameWithoutSuffix}Routes";
}
