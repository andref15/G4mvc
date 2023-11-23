﻿namespace G4mvc.Generator;
internal class ControllerRouteClassGenerator
{
    private readonly Configuration _configuration;

    public ControllerRouteClassGenerator(Configuration configuration)
    {
        _configuration = configuration;
    }

    internal void AddSharedController(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames)
    {
        var sourceBuilder = _configuration.CreateSourceBuilder();

        AddClassNameToDictionary(controllerRouteClassNames, null, "Shared", $"SharedRoutes");

        sourceBuilder.Nullable(_configuration.GlobalNullable);

        using (sourceBuilder.BeginNamespace(Configuration.RoutesNameSpace, true))
        using (sourceBuilder.BeginClass("public", "SharedRoutes"))
        {
            sourceBuilder.AppendProperty("public", "SharedViews", "Views", "get", null, SourceCode.NewCtor);
            AddViewsClass(sourceBuilder, projectDir, null, "Shared");
        }

        context.AddGeneratedSource("SharedRoutes", sourceBuilder);
    }

    internal void AddControllerRouteClass(SourceProductionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, List<ControllerDeclarationContext> controllerContexts)
    {
        var mainControllerContext = controllerContexts[0];

        var sourceBuilder = _configuration.CreateSourceBuilder();

        var httpMethods = controllerContexts.SelectMany(cc => cc.Syntax.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Select(md => new MethodDeclarationContext(md, cc.Model, _configuration.GlobalNullable))
                .Where(mc => IsActionResult(mc.MethodSymbol.ReturnType))).ToList();

        sourceBuilder
            .Using(nameof(G4mvc))
            .AppendLine()
            .Nullable(mainControllerContext.NullableEnabled);

        var controllerRouteClassName = $"{mainControllerContext.ControllerNameWithoutSuffix}Routes";
        AddClassNameToDictionary(controllerRouteClassNames, mainControllerContext.ControllerArea, mainControllerContext.ControllerNameWithoutSuffix, controllerRouteClassName);

        using (sourceBuilder.BeginNamespace(Configuration.RoutesNameSpace, true))
        using (sourceBuilder.BeginClass(_configuration.GeneratedClassModifier, controllerRouteClassName))
        {
            if (mainControllerContext.ControllerArea is not null)
            {
                sourceBuilder.AppendProperty("public", "string", "Area", "get", null, SourceCode.String(mainControllerContext.ControllerArea));
            }

            sourceBuilder
                .AppendProperty("public", "string", "Name", "get", null, SourceCode.String(mainControllerContext.ControllerNameWithoutSuffix))
                .AppendProperty("public", $"{mainControllerContext.ControllerNameWithoutSuffix}ActionNames", "ActionNames", "get", null, SourceCode.NewCtor)
                .AppendProperty("public", $"{mainControllerContext.ControllerNameWithoutSuffix}Views", "Views", "get", null, SourceCode.NewCtor);

            var httpMethodGroups = httpMethods.GroupBy(md => md.Syntax.Identifier.Text.RemoveEnd("Async")).ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var actionName in httpMethodGroups.Keys)
            {
                sourceBuilder.AppendProperty("public", $"{actionName}ParamsClass", $"{actionName}Params", "get", null, SourceCode.NewCtor);
            }

            sourceBuilder.AppendLine();

            Dictionary<string, HashSet<string>> actionParameterGroups = new();

            foreach (var httpMethodsGroup in httpMethods.GroupBy(md => md.Syntax.Identifier.Text.RemoveEnd("Async")))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                var actionName = httpMethodsGroup.Key;

                HashSet<string> methodsGroupParameterNames = new();
                actionParameterGroups.Add(actionName, methodsGroupParameterNames);

                using (sourceBuilder.BeginMethod("public", nameof(G4mvcRouteValues), actionName))
                {
                    sourceBuilder.AppendReturnCtor(nameof(G4mvcRouteValues), SourceCode.String(mainControllerContext.ControllerArea), SourceCode.String(mainControllerContext.ControllerNameWithoutSuffix), SourceCode.String(actionName));
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

                    SourceBuilder.NullableBlock? nullableBlock = null;

                    if (mainControllerContext.NullableEnabled != httpMethodContext.NullableEnabled)
                    {
                        nullableBlock = sourceBuilder.BeginNullable(httpMethodContext.NullableEnabled);
                    }

                    using (nullableBlock)
                    using (sourceBuilder.BeginMethod("public", nameof(G4mvcRouteValues), actionName, string.Join(", ", relevantParameters.Select(p => $"{p.Symbol.Type} {p.Symbol.Name}{GetDefaultValue(p.Syntax)}"))))
                    {
                        sourceBuilder.AppendLine($"{nameof(G4mvcRouteValues)} route = {actionName}()").AppendLine();

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

            using (sourceBuilder.BeginClass("public", $"{mainControllerContext.ControllerNameWithoutSuffix}ActionNames"))
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

            AddViewsClass(sourceBuilder, projectDir, mainControllerContext.ControllerArea, mainControllerContext.ControllerNameWithoutSuffix);
        }

        context.AddGeneratedSource($"{mainControllerContext.ControllerNameWithoutSuffix}Routes", sourceBuilder);
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
            controllerRouteClassNames[controllerArea ?? string.Empty] = new();
        }

        controllerRouteClassNames[controllerArea ?? string.Empty].Add(controllerRouteClassName, controllerNameWithoutSuffix);
    }

    private static void AddViewsClass(SourceBuilder sourceBuilder, string projectDir, string? controllerArea, string controllerNameWithoutSuffix)
    {
        using (sourceBuilder.BeginClass("public", $"{controllerNameWithoutSuffix}Views"))
        {
            sourceBuilder.AppendProperty("public", $"{controllerNameWithoutSuffix}ViewNames", "ViewNames", "get", null, SourceCode.NewCtor);

            List<string> viewNames = new();
            foreach (var view in GetViewsForController(projectDir, controllerArea, controllerNameWithoutSuffix))
            {
                viewNames.Add(view.Key);
                sourceBuilder.AppendProperty("public", "string", view.Key, "get", null, SourceCode.String(view.Value));
            }

            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginClass("public", $"{controllerNameWithoutSuffix}ViewNames"))
            {
                foreach (var viewName in viewNames)
                {
                    sourceBuilder.AppendProperty("public", "string", viewName, "get", null, SourceCode.Nameof(viewName));
                }
            }
        }
    }

    private static IEnumerable<KeyValuePair<string, string>> GetViewsForController(string projectDir, string? area, string controller)
    {
        var root = area is null ? Path.Combine(projectDir, "Views", controller) : Path.Combine(projectDir, "Areas", area, "Views", controller);
        DirectoryInfo directory = new(root);

        if (!directory.Exists)
        {
            yield break;
        }

        foreach (var file in directory.GetFiles("*.cshtml"))
        {
            yield return new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(file.Name), file.FullName.Replace(projectDir, "~/").Replace("\\", "/"));
        }
    }
}
