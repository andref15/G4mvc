namespace G4mvc.Generator;
internal static class ControllerRouteClassGenerator
{
    internal static void AddSharedController(GeneratorExecutionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames)
    {
        SourceBuilder sourceBuilder = Configuration.Instance.CreateSourceBuilder();

        AddClassNameToDictionary(controllerRouteClassNames, null, "Shared", "SharedRoutes");

        sourceBuilder.Nullable(Configuration.Instance.GlobalNullable);

        using (sourceBuilder.BeginNamespace($"{nameof(G4mvc)}.Routes", true))
        using (sourceBuilder.BeginClass("public", "SharedRoutes"))
        {
            sourceBuilder.AppendProperty("public", "SharedViews", "Views", "get", null, SourceCode.NewCtor);
            AddViewsClass(sourceBuilder, projectDir, null, "Shared");
        }

        context.AddGeneratedSource("SharedRoutes", sourceBuilder);
    }

    internal static void AddControllerRouteClass(GeneratorExecutionContext context, string projectDir, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, ControllerDeclarationContext controllerContext)
    {
        SourceBuilder sourceBuilder = Configuration.Instance.CreateSourceBuilder();

        string? controllerArea = GetControllerArea(controllerContext.TypeSymbol);

        List<MethodDeclarationContext> httpMethods = controllerContext.Syntax.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(md => new MethodDeclarationContext(md, controllerContext.Model))
                .Where(mc => IsActionResult(mc.MethodSymbol.ReturnType)).ToList();

        IEnumerable<string> parameterNamespaces = httpMethods.SelectMany(mc => mc.MethodSymbol.Parameters.Select(p => p.Type.ContainingNamespace.ToDisplayString())).Distinct();

        sourceBuilder
            .Using(nameof(G4mvc))
            .Using(parameterNamespaces)
            .AppendLine()
            .Nullable(controllerContext.NullableEnabled);

        string controllerRouteClassName = $"{controllerContext.ControllerNameWithoutSuffix}Routes";
        AddClassNameToDictionary(controllerRouteClassNames, controllerArea, controllerContext.ControllerNameWithoutSuffix, controllerRouteClassName);

        using (sourceBuilder.BeginNamespace($"{nameof(G4mvc)}.Routes", true))
        using (sourceBuilder.BeginClass("public", controllerRouteClassName))
        {
            if (controllerArea is not null)
            {
                sourceBuilder.AppendProperty("public", "string", "Area", "get", null, SourceCode.String(controllerArea));
            }

            sourceBuilder
                .AppendProperty("public", "string", "Name", "get", null, SourceCode.String(controllerContext.ControllerNameWithoutSuffix))
                .AppendProperty("public", $"{controllerContext.ControllerNameWithoutSuffix}ActionNames", "ActionNames", "get", null, SourceCode.NewCtor)
                .AppendProperty("public", $"{controllerContext.ControllerNameWithoutSuffix}Views", "Views", "get", null, SourceCode.NewCtor)
                .AppendLine();

            List<string> actionNames = new();

            foreach (IGrouping<string, MethodDeclarationContext> httpMethodsGroup in httpMethods.GroupBy(md => md.Syntax.Identifier.Text.RemoveEnd("Async")))
            {
                string actionName = httpMethodsGroup.Key;

                if (!actionNames.Contains(actionName))
                {
                    actionNames.Add(actionName);
                }

                using (sourceBuilder.BeginMethod("public", nameof(G4mvcRouteValues), actionName))
                {
                    sourceBuilder.AppendReturnCtor(nameof(G4mvcRouteValues), SourceCode.String(controllerArea), SourceCode.String(controllerContext.ControllerNameWithoutSuffix), SourceCode.String(actionName));
                }

                sourceBuilder.AppendLine();

                foreach (MethodDeclarationContext httpMethodContext in httpMethodsGroup.Where(md => md.Syntax.ParameterList.Parameters.Count > 0))
                {
                    List<ParameterContext> relevantParameters = httpMethodContext.Syntax.ParameterList.Parameters.Select(p => new ParameterContext(p, controllerContext.Model.GetDeclaredSymbol(p)!)).Where(p => p.Symbol.Type.ToDisplayString() != TypeNames.CancellationToken).ToList();

                    if (relevantParameters.Count is 0)
                    {
                        continue;
                    }

                    SourceBuilder.NullableBlock? nullableBlock = null;

                    if (controllerContext.NullableEnabled != httpMethodContext.NullableEnabled)
                    {
                        nullableBlock = sourceBuilder.BeginNullable(httpMethodContext.NullableEnabled);
                    }

                    using (nullableBlock)
                    using (sourceBuilder.BeginMethod("public", nameof(G4mvcRouteValues), actionName, string.Join(", ", relevantParameters.Select(p => $"{p.Symbol} {p.Symbol.Name}{GetDefaultValue(p.Syntax)}"))))
                    {
                        sourceBuilder.AppendLine($"{nameof(G4mvcRouteValues)} route = {actionName}()").AppendLine();

                        foreach (ParameterContext parameter in relevantParameters)
                        {
                            sourceBuilder.AppendLine($"route[{SourceCode.String(parameter.Symbol.Name)}] = {parameter.Symbol.Name}");
                        }

                        sourceBuilder.AppendLine().AppendLine("return route");
                    }

                    sourceBuilder.AppendLine();
                }
            }

            using (sourceBuilder.BeginClass("public", $"{controllerContext.ControllerNameWithoutSuffix}ActionNames"))
            {
                foreach (string actionName in actionNames)
                {
                    sourceBuilder.AppendProperty("public", "string", actionName, "get", null, SourceCode.Nameof(actionName));
                }
            }

            sourceBuilder.AppendLine();

            AddViewsClass(sourceBuilder, projectDir, controllerArea, controllerContext.ControllerNameWithoutSuffix);
        }

        context.AddGeneratedSource($"{controllerContext.ControllerNameWithoutSuffix}Routes", sourceBuilder);
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
            foreach (KeyValuePair<string, string> view in GetViewsForController(projectDir, controllerArea, controllerNameWithoutSuffix))
            {
                viewNames.Add(view.Key);
                sourceBuilder.AppendProperty("public", "string", view.Key, "get", null, SourceCode.String(view.Value));
            }

            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginClass("public", $"{controllerNameWithoutSuffix}ViewNames"))
            {
                foreach (string viewName in viewNames)
                {
                    sourceBuilder.AppendProperty("public", "string", viewName, "get", null, SourceCode.Nameof(viewName));
                }
            }
        }
    }

    private static string? GetControllerArea(INamedTypeSymbol typeSymbol)
    {
        AttributeData? areaAttribute = typeSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass!.DerrivesFromType(TypeNames.AreaAttribute));

        return areaAttribute?.ConstructorArguments[0].Value as string;
    }

    private static IEnumerable<KeyValuePair<string, string>> GetViewsForController(string projectDir, string? area, string controller)
    {
        string root = area is null ? Path.Combine(projectDir, "Views", controller) : Path.Combine(projectDir, "Areas", area, "Views", controller);
        DirectoryInfo directory = new(root);

        if (!directory.Exists)
        {
            yield break;
        }

        foreach (FileInfo file in directory.GetFiles("*.cshtml"))
        {
            yield return new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(file.Name), file.FullName.Replace(projectDir, "~/").Replace("\\", "/"));
        }
    }
}
