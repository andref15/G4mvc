namespace G4mvc.Generator;
internal static class MvcClassGenerator
{
    public static void AddMvcClass(GeneratorExecutionContext context, Dictionary<string, Dictionary<string, string>> controllerRouteClassNames)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        SourceBuilder sourceBuilder = Configuration.Instance.CreateSourceBuilder();

        sourceBuilder.Using(nameof(G4mvc));

        List<string> areaNames = controllerRouteClassNames.Keys.Where(k => k != string.Empty).ToList();

        if (areaNames.Count > 0)
        {
            sourceBuilder.Using($"{nameof(G4mvc)}.Areas");
        }

        sourceBuilder.Using($"{nameof(G4mvc)}.Routes").AppendLine();

        sourceBuilder.Nullable(Configuration.Instance.GlobalNullable);

        using (sourceBuilder.BeginClass("public", Configuration.Instance.JsonConfig.HelperClassName))
        {
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

        context.AddGeneratedSource(Configuration.Instance.JsonConfig.HelperClassName, sourceBuilder);
    }
}
