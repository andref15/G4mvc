#if !NETSTANDARD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace G4mvc.Extensions;

public static class LinkGeneratorExtensions
{
    extension(LinkGenerator _linkGenerator)
    {
        public string? GetPathByAction(HttpContext httpContext, G4mvcActionRouteValues route, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetPathByAction(httpContext, route.Action, route.Controller, values: route, pathBase, fragment, options);
        }

        public string? GetPathByAction(G4mvcActionRouteValues route, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetPathByAction(route.Action, route.Controller, route, pathBase, fragment, options);
        }

        public string? GetUriByAction(HttpContext httpContext, G4mvcActionRouteValues route, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetUriByAction(httpContext, route.Action, route.Controller, route, scheme, host, pathBase, fragment, options);
        }

        public string? GetUriByAction(G4mvcActionRouteValues routeValues, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetUriByAction(routeValues.Action, routeValues.Controller, routeValues, scheme, host, pathBase, fragment, options);
        }

        public string? GetPathByPage(HttpContext httpContext, G4mvcPageRouteValues route, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetPathByPage(httpContext, route.Page, route.Handler, route, pathBase, fragment, options);
        }

        public string? GetPathByPage(G4mvcPageRouteValues route, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetPathByPage(route.Page, route.Handler, route, pathBase, fragment, options);
        }

        public string? GetUriByPage(HttpContext httpContext, G4mvcPageRouteValues route, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetUriByPage(httpContext, route.Page, route.Handler, route, scheme, host, pathBase, fragment, options);
        }

        public string? GetUriByPage(G4mvcPageRouteValues route, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        {
            return _linkGenerator.GetUriByPage(route.Page, route.Handler, route, scheme, host, pathBase, fragment, options);
        }
    }
}
#endif
