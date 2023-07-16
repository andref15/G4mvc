using Microsoft.AspNetCore.Routing;

namespace G4mvc;
public class G4mvcRouteValues : RouteValueDictionary
{
    public string? Area { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }

    public G4mvcRouteValues(string? area, string controller, string action)
    {
        Area = area;
        Controller = controller;
        Action = action;

        if (area is not null)
        {
            this[nameof(area)] = area;
        }

        this[nameof(controller)] = controller;
        this[nameof(action)] = action;
    }

    public RouteValueDictionary AsRouteValueDictionary()
        => this;

    public G4mvcRouteValues AddRouteValue(string key, object value)
    {
        this[key] = value;

        return this;
    }

    public override string ToString()
    {
#if NETCOREAPP
        int length = 3 + (Area?.Length ?? -1) + Controller.Length + Action.Length; //3 is the maximum number of slashes, if area is null count -1

        return string.Create(length, this, (span, routeValues) =>
        {
            span[0] = '/';
            int currentIdx = 1;

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
    private static void CopyPathSegmentToSpanAt(Span<char> span, ReadOnlySpan<char> value, ref int idx)
    {
        value.CopyTo(span[idx..]);
        idx += value.Length;
        span[idx++] = '/';
    }
#endif
}
