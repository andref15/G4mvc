using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;

internal static class AreaClassesGenerator
{
    internal static void AddMvcAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration)
        => AddAreaClasses(context, routeClassNames, configuration, configuration.GetMvcAreasNamespace(), configuration.GetMvcNamespace);

    internal static void AddPagesAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration)
        => AddAreaClasses(context, routeClassNames, configuration, configuration.GetPagesAreasNamespace(), configuration.GetPagesNamespace);

    private static void AddAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration, string areaNamespace, Func<string?, string> getHelperNamespace)
    {
        foreach (var (areaName, className) in routeClassNames.Where(kvp => kvp.Key != string.Empty))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var sourceBuilder = configuration.CreateSourceBuilder();

            sourceBuilder
                .Using(getHelperNamespace(areaName))
                .Nullable(configuration.GlobalNullable);

            using (sourceBuilder.BeginNamespace(areaNamespace, true))
            using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, $"{areaName}Area"))
            {
                sourceBuilder.AppendProperty("public", "string", "Name", "get", null, SourceCode.String(areaName));
                sourceBuilder.AppendProperties("public", className, "get", null, "new()");
            }

            context.AddGeneratedSource($"{areaName}Area", sourceBuilder);
        }
    }
}
