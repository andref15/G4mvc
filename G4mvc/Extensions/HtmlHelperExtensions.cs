#if NETCOREAPP
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#endif

namespace G4mvc.Extensions;
public static class HtmlHelperExtensions
{
#if NETCOREAPP
    public static IHtmlContent ActionLink(this IHtmlHelper htmlHelper, string linkText, G4mvcRouteValues route, object? htmlAttributes = null, string? protocol = null, string? hostName = null, string? fragment = null)
    => RouteLink(htmlHelper, linkText, null, route, htmlAttributes, protocol, hostName, fragment);

    public static IHtmlContent ActionLink(this IHtmlHelper htmlHelper, string linkText, G4mvcRouteValues route, IDictionary<string, object> htmlAttributes, string? protocol = null, string? hostName = null, string? fragment = null)
        => RouteLink(htmlHelper, linkText, null, route, htmlAttributes, protocol, hostName, fragment);

    public static IHtmlContent RouteLink(this IHtmlHelper htmlHelper, string linkText, G4mvcRouteValues route, object? htmlAttributes)
        => RouteLink(htmlHelper, linkText, null, route, htmlAttributes, null, null, null);

    public static IHtmlContent RouteLink(this IHtmlHelper htmlHelper, string linkText, G4mvcRouteValues route, IDictionary<string, object> htmlAttributes)
        => RouteLink(htmlHelper, linkText, null, route, htmlAttributes, null, null, null);

    public static IHtmlContent RouteLink(this IHtmlHelper htmlHelper, string linkText, string? routeName, G4mvcRouteValues route, object? htmlAttributes, string? protocol = null, string? hostName = null, string? fragment = null)
        => RouteLink(htmlHelper, linkText, routeName, route, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), protocol, hostName, fragment);

    public static IHtmlContent RouteLink(this IHtmlHelper htmlHelper, string linkText, string? routeName, G4mvcRouteValues route, IDictionary<string, object> htmlAttributes, string? protocol = null, string? hostName = null, string? fragment = null)
        => htmlHelper.RouteLink(linkText, routeName, protocol, hostName, fragment, route.AsRouteValueDictionary(), htmlAttributes);

    public static MvcForm BeginForm(this IHtmlHelper htmlHelper, G4mvcRouteValues route, object? htmlAttributes = null)
        => BeginForm(htmlHelper, route, FormMethod.Post, htmlAttributes);

    public static MvcForm BeginForm(this IHtmlHelper htmlHelper, G4mvcRouteValues route, FormMethod formMethod, object? htmlAttributes = null)
        => BeginForm(htmlHelper, route, formMethod, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

    public static MvcForm BeginForm(this IHtmlHelper htmlHelper, G4mvcRouteValues route, FormMethod formMethod, IDictionary<string, object> htmlAttributes)
        => BeginRouteForm(htmlHelper, null, route, formMethod, htmlAttributes);

    public static MvcForm BeginRouteForm(this IHtmlHelper htmlHelper, G4mvcRouteValues route, object? htmlAttributes = null)
        => BeginRouteForm(htmlHelper, null, route, FormMethod.Post, htmlAttributes);

    public static MvcForm BeginRouteForm(this IHtmlHelper htmlHelper, string? routeName, G4mvcRouteValues route, FormMethod formMethod, object? htmlAttributes = null)
        => BeginRouteForm(htmlHelper, routeName, route, formMethod, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

    public static MvcForm BeginRouteForm(this IHtmlHelper htmlHelper, string? routeName, G4mvcRouteValues route, FormMethod formMethod, IDictionary<string, object> htmlAttributes)
        => htmlHelper.BeginRouteForm(routeName, route, formMethod, null, htmlAttributes);
#endif
}
