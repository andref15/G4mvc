using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;
internal static class ControllerPartialClassGenerator
{
    internal static void AddControllerPartialClass(SourceProductionContext context, ControllerDeclarationContext controllerContext, Configuration configuration)
    {
        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder.Using(nameof(G4mvc), Namespaces.MicrosoftAspNetCoreMvc);

        if (configuration.GeneratedClassNamespace is not null)
        {
            sourceBuilder.Using(configuration.GeneratedClassNamespace);
        }

        sourceBuilder
            .AppendLine()
            .Nullable(controllerContext.NullableEnabled);

        using (sourceBuilder.BeginNamespace(controllerContext.TypeSymbol.ContainingNamespace.ToDisplayString(), true))
        using (sourceBuilder.BeginClass(controllerContext.DeclarationNode.Modifiers.ToString(), controllerContext.TypeSymbol.Name))
        {
            sourceBuilder.AppendProperty($"{(configuration.JsonConfig.MakeGeneratedClassesInternal ? "private " : null)}protected", $"global::{configuration.GetMvcNamespace(controllerContext.Area)}.{controllerContext.NameWithoutSuffix}Routes.{controllerContext.NameWithoutSuffix}Views", "Views", $"get", null, $"{configuration.JsonConfig.MvcHelperClassName}.{(controllerContext.Area is null ? null : $"{controllerContext.Area}.")}{controllerContext.NameWithoutSuffix}.Views");
            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginMethod("protected", "RedirectToRouteResult", "RedirectToAction", $"{nameof(G4mvcActionRouteValues)} route"))
            {
                sourceBuilder.AppendReturn("RedirectToRoute(route)");
            }

            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginMethod("protected", "RedirectToRouteResult", "RedirectToActionPermanent", $"{nameof(G4mvcActionRouteValues)} route"))
            {
                sourceBuilder.AppendReturn("RedirectToRoutePermanent(route)");
            }
        }

        context.AddGeneratedSource(GetPartialClassName(controllerContext), sourceBuilder);
    }

    private static string GetPartialClassName(ControllerDeclarationContext controllerContext)
    {
        var area = controllerContext.Area;

        return area is null
            ? $"{controllerContext.Name}"
            : $"{area}.{controllerContext.Name}";
    }
}
