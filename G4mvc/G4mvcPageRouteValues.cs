#if !NETSTANDARD
using G4mvc.Extensions;
using G4mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
#endif

namespace G4mvc;

public class G4mvcPageRouteValues : G4mvcBaseRouteValues<G4mvcPageRouteValues>
{
    private const string _pageKey = "page";
    private const string _handlerKey = "handler";

    public string Page { get => (string)this[_pageKey]!; set => this[_pageKey] = value; }
    public string? Handler { get => (string?)this[_handlerKey]; set => this[_handlerKey] = value; }
    public string Method { get; set; }

    public G4mvcPageRouteValues(string? area, string page, string? handler, string method) : base(area)
    {
        Page = page;
        Handler = handler;
        Method = method;
    }

    /// <summary>
    /// Formats the route values into a <see cref="string"/> representation.
    /// Do not use this for navigation purposes! Use <see cref="ToString(IUrlHelper)"/> instead.
    /// </summary>
    /// <returns>The route formatted as [Area]/[Page]</returns>
    public override string ToString()
    {
#if !NETSTANDARD
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
}
