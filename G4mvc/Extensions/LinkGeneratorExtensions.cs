#if NETCOREAPP
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;

namespace G4mvc.Extensions;

public static class LinkGeneratorExtensions
{

    public static string? GetPathByAction(this LinkGenerator linkGenerator, HttpContext httpContext, G4mvcRouteValues routeValues, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        var address = CreateAddress(httpContext, routeValues);
        return linkGenerator.GetPathByAddress(httpContext, address: address, values: address.ExplicitValues, ambientValues: address.AmbientValues, pathBase, fragment, options);
    }

    public static string? GetPathByAction(this LinkGenerator linkGenerator, G4mvcRouteValues routeValues, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        var address = CreateAddress(null, routeValues);
        return linkGenerator.GetPathByAddress(address: address, values: address.ExplicitValues, pathBase, fragment, options);
    }

    public static string? GetUriByAction(this LinkGenerator linkGenerator, HttpContext httpContext, G4mvcRouteValues routeValues, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        var address = CreateAddress(httpContext, routeValues);
        return linkGenerator.GetUriByAddress(httpContext, address, values: address.ExplicitValues, ambientValues: address.AmbientValues, scheme, host, pathBase, fragment, options);
    }

    public static string? GetUriByAction(this LinkGenerator linkGenerator, G4mvcRouteValues routeValues, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        var address = CreateAddress(null, routeValues);
        return linkGenerator.GetUriByAddress(address, values: address.ExplicitValues, scheme, host, pathBase, fragment, options);
    }



    private static RouteValuesAddress CreateAddress(HttpContext? httpContext, G4mvcRouteValues routeValues)
    {
        var ambientValues = httpContext?.Features?.Get<IRouteValuesFeature>()?.RouteValues;

        return new RouteValuesAddress
        {
            ExplicitValues = routeValues,
            AmbientValues = ambientValues
        };
    }

}
#endif
