#if NETCOREAPP
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace G4mvc.Extensions;
public static class UrlHelperExtensions
{
    public static string? Action(this IUrlHelper urlHelper, G4mvcRouteValues route)
        => urlHelper.RouteUrl(route);

    public static string? Action(this IUrlHelper urlHelper, G4mvcRouteValues route, string? protocol = null, string? hostName = null, string? fragment = null)
        => urlHelper.RouteUrl(null, route, protocol, hostName, fragment);

    public static string ActionAbsolute(this IUrlHelper urlHelper, G4mvcRouteValues route)
    {
        var request = urlHelper.ActionContext.HttpContext.Request;
        return $"{request.Scheme}://{request.Host}{urlHelper.RouteUrl(route)}";
    }

    public static string? RouteUrl(this IUrlHelper urlHelper, G4mvcRouteValues route)
        => urlHelper.RouteUrl(null, route, null, null);

    public static string? RouteUrl(this IUrlHelper urlHelper, string? routeName, G4mvcRouteValues route, string? protocol = null, string? hostName = null, string? fragment = null)
        => urlHelper.RouteUrl(routeName, (RouteValueDictionary)route, protocol, hostName, fragment);

    public static string? Content(this IUrlHelper urlHelper, G4mvcContentLink contentLink)
        => contentLink.ToContentUrl(urlHelper);
}

#endif