namespace G4mvc.Generator;
internal static class AreaClassesGenerator
{
    internal static void AddAreaClasses(GeneratorExecutionContext context, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames)
    {
        foreach (KeyValuePair<string, Dictionary<string, string>> area in controllerRouteClassNames.Where(kvp => kvp.Key != string.Empty))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            SourceBuilder sourceBuilder = Configuration.Instance.CreateSourceBuilder();

            sourceBuilder
                .Using($"{nameof(G4mvc)}.Routes")
                .Nullable(Configuration.Instance.GlobalNullable);

            using (sourceBuilder.BeginNamespace($"{nameof(G4mvc)}.Areas", true))
            using (sourceBuilder.BeginClass("public", $"{area.Key}Area"))
            {
                sourceBuilder.AppendProperties("public", area.Value, "get", null, "new()");
            }

            context.AddGeneratedSource($"{area.Key}Area", sourceBuilder);
        }
    }
}
