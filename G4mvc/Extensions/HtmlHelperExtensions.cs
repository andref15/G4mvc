#if NETCOREAPP
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#endif

namespace G4mvc.Extensions;

public static class HtmlHelperExtensions
{
#if NETCOREAPP
    extension(IHtmlHelper _htmlHelper)
    {

        public IHtmlContent ActionLink(string linkText, G4mvcActionRouteValues route, object? htmlAttributes = null, string? protocol = null, string? hostName = null, string? fragment = null)
        => RouteLink(_htmlHelper, linkText, null, route, htmlAttributes, protocol, hostName, fragment);

        public IHtmlContent ActionLink(string linkText, G4mvcActionRouteValues route, IDictionary<string, object> htmlAttributes, string? protocol = null, string? hostName = null, string? fragment = null)
            => RouteLink(_htmlHelper, linkText, null, route, htmlAttributes, protocol, hostName, fragment);

        public IHtmlContent RouteLink(string linkText, G4mvcActionRouteValues route, object? htmlAttributes)
            => RouteLink(_htmlHelper, linkText, null, route, htmlAttributes, null, null, null);

        public IHtmlContent RouteLink(string linkText, G4mvcActionRouteValues route, IDictionary<string, object> htmlAttributes)
            => RouteLink(_htmlHelper, linkText, null, route, htmlAttributes, null, null, null);

        public IHtmlContent RouteLink(string linkText, string? routeName, G4mvcActionRouteValues route, object? htmlAttributes, string? protocol = null, string? hostName = null, string? fragment = null)
            => RouteLink(_htmlHelper, linkText, routeName, route, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), protocol, hostName, fragment);

        public IHtmlContent RouteLink(string linkText, string? routeName, G4mvcActionRouteValues route, IDictionary<string, object> htmlAttributes, string? protocol = null, string? hostName = null, string? fragment = null)
            => _htmlHelper.RouteLink(linkText, routeName, protocol, hostName, fragment, route.AsRouteValueDictionary(), htmlAttributes);

        public MvcForm BeginForm(G4mvcActionRouteValues route, object? htmlAttributes = null)
            => BeginForm(_htmlHelper, route, FormMethod.Post, htmlAttributes);

        public MvcForm BeginForm(G4mvcActionRouteValues route, FormMethod formMethod, object? htmlAttributes = null)
            => BeginForm(_htmlHelper, route, formMethod, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

        public MvcForm BeginForm(G4mvcActionRouteValues route, FormMethod formMethod, IDictionary<string, object> htmlAttributes)
            => BeginRouteForm(_htmlHelper, null, route, formMethod, htmlAttributes);

        public MvcForm BeginRouteForm(G4mvcActionRouteValues route, object? htmlAttributes = null)
            => BeginRouteForm(_htmlHelper, null, route, FormMethod.Post, htmlAttributes);

        public MvcForm BeginRouteForm(string? routeName, G4mvcActionRouteValues route, FormMethod formMethod, object? htmlAttributes = null)
            => BeginRouteForm(_htmlHelper, routeName, route, formMethod, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

        public MvcForm BeginRouteForm(string? routeName, G4mvcActionRouteValues route, FormMethod formMethod, IDictionary<string, object> htmlAttributes)
            => _htmlHelper.BeginRouteForm(routeName, route, formMethod, null, htmlAttributes);
    }
#endif
}
