#if NETCOREAPP
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace G4mvc.Extensions;

public static class UrlHelperExtensions
{
    extension(IUrlHelper _urlHelper)
    {
        public string? Action(G4mvcActionRouteValues route)
            => _urlHelper.RouteUrl(route);

        public string? Action(G4mvcActionRouteValues route, string? protocol = null, string? hostName = null, string? fragment = null)
            => _urlHelper.RouteUrl(null, route, protocol, hostName, fragment);

        public string ActionAbsolute(G4mvcActionRouteValues route)
            => GetAbsoluteUrl(_urlHelper, route);

        public string? RouteUrl(G4mvcActionRouteValues route)
            => _urlHelper.RouteUrl(null, route, null, null);

        public string? RouteUrl(string? routeName, G4mvcActionRouteValues route, string? protocol = null, string? hostName = null, string? fragment = null)
            => _urlHelper.RouteUrl(routeName, route, protocol, hostName, fragment);

        public string? Content(G4mvcContentLink contentLink)
            => contentLink.ToContentUrl(_urlHelper);

        public string? Page(G4mvcPageRouteValues route)
            => _urlHelper.RouteUrl(route);

        public string? Page(G4mvcPageRouteValues route, string? protocol = null, string? hostName = null, string? fragment = null)
            => _urlHelper.RouteUrl(null, route, protocol, hostName, fragment);

        public string PageAbsolute(G4mvcPageRouteValues route)
            => GetAbsoluteUrl(_urlHelper, route);

        private string GetAbsoluteUrl(RouteValueDictionary route)
        {
            var request = _urlHelper.ActionContext.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{_urlHelper.RouteUrl(route)}";
        }
    }
}
#endif