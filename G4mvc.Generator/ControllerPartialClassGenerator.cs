﻿namespace G4mvc.Generator;
internal static class ControllerPartialClassGenerator
{
    internal static void AddControllerPartialClass(SourceProductionContext context, ControllerDeclarationContext controllerContext, Configuration configuration)
    {
        if (!controllerContext.Syntax.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            return;
        }

        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder
            .Using(nameof(G4mvc), Namespaces.MicrosoftAspNetCoreMvc).AppendLine()
            .Nullable(controllerContext.NullableEnabled);

        using (sourceBuilder.BeginNamespace(controllerContext.TypeSymbol.ContainingNamespace.ToDisplayString(), true))
        using (sourceBuilder.BeginClass(controllerContext.Syntax.Modifiers.ToString(), controllerContext.TypeSymbol.Name))
        {
            sourceBuilder.AppendProperty("protected", $"{Configuration.RoutesNameSpace}.{controllerContext.ControllerNameWithoutSuffix}Routes.{controllerContext.ControllerNameWithoutSuffix}Views", "Views", $"get", null, $"{configuration.JsonConfig.HelperClassName}.{(controllerContext.ControllerArea is null ? null : $"{controllerContext.ControllerArea}.")}{controllerContext.ControllerNameWithoutSuffix}.Views");
            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginMethod("protected", "RedirectToRouteResult", "RedirectToAction", "G4mvcRouteValues route"))
            {
                sourceBuilder.AppendReturn("RedirectToRoute(route)");
            }

            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginMethod("protected", "RedirectToRouteResult", "RedirectToActionPermanent", "G4mvcRouteValues route"))
            {
                sourceBuilder.AppendReturn("RedirectToRoutePermanent(route)");
            }
        }

        context.AddGeneratedSource($"{controllerContext.ControllerName}", sourceBuilder);
    }
}
