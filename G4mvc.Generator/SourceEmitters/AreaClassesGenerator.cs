using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;

internal static class AreaClassesGenerator
{
    internal static void AddMvcAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration)
        => AddAreaClasses(context, routeClassNames, configuration, "Mvc", configuration.GetMvcAreasNamespace(), configuration.GetMvcNamespace);

    internal static void AddPagesAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration)
        => AddAreaClasses(context, routeClassNames, configuration, "Page", configuration.GetPagesAreasNamespace(), configuration.GetPagesNamespace);

    private static void AddAreaClasses(SourceProductionContext context, Dictionary<string, Dictionary<string, string>> routeClassNames, Configuration configuration, string type, string areaNamespace, Func<string?, string> getHelperNamespace)
    {
        foreach (var (areaName, classNames) in routeClassNames.Where(kvp => kvp.Key != string.Empty))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var sourceBuilder = configuration.CreateSourceBuilder();

            sourceBuilder.Nullable(configuration.GlobalNullable);

            using (sourceBuilder.BeginNamespace(areaNamespace, true))
            using (sourceBuilder.BeginClass(configuration.GeneratedClassModifier, $"{areaName}Area"))
            {
                sourceBuilder.AppendProperty("public", "string", "Name", "get", null, SourceCode.String(areaName));

                foreach (var (classType, className) in classNames)
                {
                    sourceBuilder.AppendProperty("public", $"global::{getHelperNamespace(areaName)}.{classType}", className, "get", null, "new()");
                }

            }

            context.AddGeneratedSource($"{areaName}{type}Area", sourceBuilder);
        }
    }
}
