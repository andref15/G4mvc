namespace G4mvc.Generator;
internal static class AreaClassesGenerator
{
    internal static void AddAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames, Configuration configuration)
    {
        foreach (var area in controllerRouteClassNames.Where(kvp => kvp.Key != string.Empty))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var sourceBuilder = configuration.CreateSourceBuilder();

            sourceBuilder
                .Using(Configuration.RoutesNameSpace)
                .Nullable(configuration.GlobalNullable);

            using (sourceBuilder.BeginNamespace($"{nameof(G4mvc)}.Areas", true))
            using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, $"{area.Key}Area"))
            {
                sourceBuilder.AppendProperty("public", "string", "Name", "get", null, SourceCode.String(area.Key));
                sourceBuilder.AppendProperties("public", area.Value, "get", null, "new()");
            }

            context.AddGeneratedSource($"{area.Key}Area", sourceBuilder);
        }
    }
}
