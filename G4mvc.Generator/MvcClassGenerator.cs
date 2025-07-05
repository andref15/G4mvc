namespace G4mvc.Generator;
internal static class MvcClassGenerator
{
    public static void AddMvcClass(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, Configuration configuration
#if DEBUG
        , int version
#endif
        )
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder.Using(nameof(G4mvc));

        var areaNames = controllerRouteClassNames.Keys.Where(k => k != string.Empty).ToList();

        if (areaNames.Count > 0)
        {
            sourceBuilder.Using($"{nameof(G4mvc)}.Areas");
        }

        sourceBuilder.Using(Configuration.RoutesNameSpace).AppendLine();

        IDisposable? namespaceDisposable = null;

        if (configuration.GeneratedClassNamespace is not null)
        {
            namespaceDisposable = sourceBuilder.BeginNamespace(configuration.GeneratedClassNamespace, true);
        }

        sourceBuilder.Nullable(configuration.GlobalNullable);

        using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, configuration.JsonConfig.HelperClassName))
        {
#if DEBUG
            sourceBuilder.AppendLine($"//v{version}");
#endif

            if (controllerRouteClassNames.TryGetValue(string.Empty, out var classNames))
            {
                sourceBuilder.AppendProperties("public static", classNames, "get", null, SourceCode.NewCtor);
            }

            foreach (var areaName in areaNames)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                sourceBuilder.AppendProperty("public static", $"{areaName}Area", areaName, "get", null, SourceCode.NewCtor);
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        namespaceDisposable?.Dispose();

        context.AddGeneratedSource(configuration.JsonConfig.HelperClassName, sourceBuilder);
    }
}
