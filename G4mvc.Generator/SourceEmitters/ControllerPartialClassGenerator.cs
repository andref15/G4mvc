using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;

internal static class ControllerPartialClassGenerator
{
    internal static void AddControllerPartialClass(SourceProductionContext context, ControllerDeclarationContext controllerContext, Configuration configuration)
    {
        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder.Nullable(controllerContext.NullableEnabled);

        using (sourceBuilder.BeginNamespace(controllerContext.TypeSymbol.ContainingNamespace.ToDisplayString(), true))
        using (sourceBuilder.BeginClass(controllerContext.DeclarationNode.Modifiers.ToString(), controllerContext.TypeSymbol.Name))
        {
            var helperNamespace = configuration.GeneratedClassNamespace;

            if (helperNamespace is not null)
            {
                helperNamespace += '.';
            }

            sourceBuilder.AppendProperty($"{(configuration.JsonConfig.MakeGeneratedClassesInternal ? "private " : null)}protected", $"global::{configuration.GetMvcNamespace(controllerContext.Area)}.{controllerContext.NameWithoutSuffix}Routes.{controllerContext.NameWithoutSuffix}Views", "Views", $"get", null, $"global::{helperNamespace}{configuration.JsonConfig.MvcHelperClassName}.{(controllerContext.Area is null ? null : $"{controllerContext.Area}.")}{controllerContext.NameWithoutSuffix}.Views");
            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginMethod("protected", $"global::{Namespaces.MicrosoftAspNetCoreMvc}.RedirectToRouteResult", "RedirectToAction", $"global::{nameof(G4mvc)}.{nameof(G4mvcActionRouteValues)} route"))
            {
                sourceBuilder.AppendReturn("RedirectToRoute(route)");
            }

            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginMethod("protected", $"global::{Namespaces.MicrosoftAspNetCoreMvc}.RedirectToRouteResult", "RedirectToActionPermanent", $"global::{nameof(G4mvc)}.{nameof(G4mvcActionRouteValues)} route"))
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
