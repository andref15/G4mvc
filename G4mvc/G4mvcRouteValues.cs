#if NETCOREAPP
using G4mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
#endif

namespace G4mvc;
public class G4mvcRouteValues
#if NETCOREAPP
        : RouteValueDictionary
#endif
{
    public string? Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }

    public G4mvcRouteValues(string? area, string controller, string action)
    {
        Area = area;
        Controller = controller;
        Action = action;

#if NETCOREAPP
        if (area is not null)
        {
            this[nameof(area)] = area;
        }

        this[nameof(controller)] = controller;
        this[nameof(action)] = action;
#endif
    }
#if NETCOREAPP

    public RouteValueDictionary AsRouteValueDictionary()
        => this;

    public G4mvcRouteValues AddRouteValue(string key, object value)
    {
        this[key] = value;

        return this;
    }
#endif

    /// <summary>
    /// Formats the route values into a <see cref="string"/> representation.
    /// Do not use this for navigation purposes! Use <see cref="ToString(IUrlHelper)"/> instead.
    /// </summary>
    /// <returns>The route formatted as [Area]/[Controller]/[Action]</returns>
    public override string ToString()
    {
#if NETCOREAPP
        var length = 3 + (Area?.Length ?? -1) + Controller.Length + Action.Length; //3 is the maximum number of slashes, if area is null count -1

        return string.Create(length, this, (span, routeValues) =>
        {
            span[0] = '/';
            var currentIdx = 1;

            if (routeValues.Area is not null)
            {
                CopyPathSegmentToSpanAt(span, routeValues.Area, ref currentIdx);
            }

            CopyPathSegmentToSpanAt(span: span, routeValues.Controller, ref currentIdx);

            routeValues.Action.AsSpan().CopyTo(span[currentIdx..]);
        });
#else
        return $"/{(Area is null ? null : $"{Area}/")}{Controller}/{Action}";
#endif
    }

#if NETCOREAPP
    public string? ToString(IUrlHelper urlHelper)
        => urlHelper.RouteUrl(this);

    private static void CopyPathSegmentToSpanAt(Span<char> span, ReadOnlySpan<char> value, ref int idx)
    {
        value.CopyTo(span[idx..]);
        idx += value.Length;
        span[idx++] = '/';
    }
#endif
}
