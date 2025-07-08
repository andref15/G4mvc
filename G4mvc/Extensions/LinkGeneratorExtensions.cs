#if NETCOREAPP
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace G4mvc.Extensions;

public static class LinkGeneratorExtensions
{

    public static string? GetPathByAction(this LinkGenerator linkGenerator, HttpContext httpContext, G4mvcRouteValues routeValues, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return linkGenerator.GetPathByAction(httpContext, routeValues.Action, routeValues.Controller, values: routeValues, pathBase, fragment, options);
    }

    public static string? GetPathByAction(this LinkGenerator linkGenerator, G4mvcRouteValues routeValues, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return linkGenerator.GetPathByAction(routeValues.Action, routeValues.Controller, routeValues, pathBase, fragment, options);
    }

    public static string? GetUriByAction(this LinkGenerator linkGenerator, HttpContext httpContext, G4mvcRouteValues routeValues, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        return linkGenerator.GetUriByAction(httpContext, routeValues.Action, routeValues.Controller, routeValues, scheme, host, pathBase, fragment, options);
    }

    public static string? GetUriByAction(this LinkGenerator linkGenerator, G4mvcRouteValues routeValues, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return linkGenerator.GetUriByAction(routeValues.Action, routeValues.Controller, routeValues, scheme, host, pathBase, fragment, options);
    }
}
#endif
