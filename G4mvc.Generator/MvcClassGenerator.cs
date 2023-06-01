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

        SourceBuilder sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder.Using(nameof(G4mvc));

        List<string> areaNames = controllerRouteClassNames.Keys.Where(k => k != string.Empty).ToList();

        if (areaNames.Count > 0)
        {
            sourceBuilder.Using($"{nameof(G4mvc)}.Areas");
        }

        sourceBuilder.Using($"{nameof(G4mvc)}.Routes").AppendLine();

        sourceBuilder.Nullable(configuration.GlobalNullable);

        using (sourceBuilder.BeginClass("public", configuration.JsonConfig.HelperClassName))
        {
#if DEBUG
            sourceBuilder.AppendLine($"//v{version}"); 
#endif

            if (controllerRouteClassNames.TryGetValue(string.Empty, out Dictionary<string, string> classNames))
            {
                sourceBuilder.AppendProperties("public static", classNames, "get", null, SourceCode.NewCtor);
            }

            foreach (string areaName in areaNames)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                sourceBuilder.AppendProperty("public static", $"{areaName}Area", areaName, "get", null, SourceCode.NewCtor);
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        context.AddGeneratedSource(configuration.JsonConfig.HelperClassName, sourceBuilder);
    }
}
