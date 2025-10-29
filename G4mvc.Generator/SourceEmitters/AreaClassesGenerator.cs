using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;
internal static class AreaClassesGenerator
{
    internal static void AddAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration)
    {
        foreach (var (areaName, className) in routeClassNames.Where(kvp => kvp.Key != string.Empty))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var sourceBuilder = configuration.CreateSourceBuilder();

            sourceBuilder
                .Using(configuration.GetMvcNamespace(areaName))
                .Nullable(configuration.GlobalNullable);

            using (sourceBuilder.BeginNamespace(configuration.GetAreasNamespace(), true))
            using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, $"{areaName}Area"))
            {
                sourceBuilder.AppendProperty("public", "string", "Name", "get", null, SourceCode.String(areaName));
                sourceBuilder.AppendProperties("public", className, "get", null, "new()");
            }

            context.AddGeneratedSource($"{areaName}Area", sourceBuilder);
        }
    }
}
