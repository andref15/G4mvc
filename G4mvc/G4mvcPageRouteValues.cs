﻿#if NETCOREAPP
using G4mvc.Extensions;
using G4mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
#endif

namespace G4mvc;
public class G4mvcPageRouteValues
#if NETCOREAPP
        : RouteValueDictionary
#else
        : Dictionary<string, object?>
#endif
{
    private const string _areaKey = "area";
    private const string _pageKey = "page";

    public string? Area { get => (string?)this[_areaKey]; set => this[_areaKey] = value; }
    public string Page { get => (string)this[_pageKey]!; set => this[_pageKey] = value; }
    public string Method { get; set; }

    public G4mvcPageRouteValues(string? area, string page, string method)
    {
        Area = area;
        Page = page;
        Method = method;
    }

#if NETCOREAPP
    public RouteValueDictionary AsRouteValueDictionary()
        => this;
#endif

    /// <summary>
    /// Formats the route values into a <see cref="string"/> representation.
    /// Do not use this for navigation purposes! Use <see cref="ToString(IUrlHelper)"/> instead.
    /// </summary>
    /// <returns>The route formatted as [Area]/[Page]</returns>
    public override string ToString()
    {
#if NETCOREAPP
        var length = 2 + (Area?.Length ?? -1) + Page.Length; //2 is the maximum number of slashes, if area is null count -1

        return string.Create(length, this, (span, routeValues) =>
        {
            span[0] = '/';
            var currentIdx = 1;

            if (routeValues.Area is not null)
            {
                SpanPath.CopyPathSegmentToSpanAt(span, routeValues.Area, ref currentIdx);
            }

            routeValues.Page.AsSpan().CopyTo(span[currentIdx..]);
        });
#else
        return $"/{(Area is null ? null : $"{Area}/")}{Page}";
#endif
    }

#if NETCOREAPP
    public string? ToString(IUrlHelper urlHelper)
        => urlHelper.RouteUrl(this);
#endif
}
