using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;
internal static class RouteHelperClassGenerator
{
    public static void AddRouteClassClass(SourceProductionContext context, string helperClassName, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration
#if DEBUG
        , int version
#endif
        )
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var sourceBuilder = configuration.CreateSourceBuilder();

        var areaNames = routeClassNames.Keys.Where(static k => !string.IsNullOrEmpty(k)).ToList();

        if (areaNames.Count > 0)
        {
            sourceBuilder.Using(configuration.GetAreasNamespace());
        }

        sourceBuilder.Using(configuration.GetControllerRoutesNamespace(null));

        var namespaceDisposable = (IDisposable?)null;

        if (configuration.GeneratedClassNamespace is not null)
        {
            namespaceDisposable = sourceBuilder.BeginNamespace(configuration.GeneratedClassNamespace, true);
        }

        sourceBuilder.Nullable(configuration.GlobalNullable);

        using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, helperClassName))
        {
#if DEBUG
            sourceBuilder.AppendLine($"//v{version}");
#endif

            if (routeClassNames.TryGetValue(string.Empty, out var classNames))
            {
                sourceBuilder.AppendProperties("public static", classNames, "get", null, SourceCode.NewCtor, context.CancellationToken);
            }

            foreach (var areaName in areaNames)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                sourceBuilder.AppendProperty("public static", $"{areaName}Area", areaName, "get", null, SourceCode.NewCtor);
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        namespaceDisposable?.Dispose();

        context.AddGeneratedSource(helperClassName, sourceBuilder);
    }
}
