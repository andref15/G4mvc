namespace G4mvc.Generator;
internal static class AreaClassesGenerator
{
    internal static void AddAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, Configuration configuration)
    {
        foreach (var (areaName, className) in controllerRouteClassNames.Where(kvp => kvp.Key != string.Empty))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var sourceBuilder = configuration.CreateSourceBuilder();

            sourceBuilder
                .Using($"{Configuration.RoutesNameSpace}.{areaName}")
                .Nullable(configuration.GlobalNullable);

            using (sourceBuilder.BeginNamespace($"{nameof(G4mvc)}.Areas", true))
            using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, $"{areaName}Area"))
            {
                sourceBuilder.AppendProperty("public", "string", "Name", "get", null, SourceCode.String(areaName));
                sourceBuilder.AppendProperties("public", className, "get", null, "new()");
            }

            context.AddGeneratedSource($"{areaName}Area", sourceBuilder);
        }
    }
}
