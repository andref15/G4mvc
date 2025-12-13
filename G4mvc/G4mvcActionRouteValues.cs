#if !NETSTANDARD
using G4mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
#endif

namespace G4mvc;

public class G4mvcActionRouteValues : G4mvcBaseRouteValues
{
    private const string _areaKey = "area";
    private const string _controllerKey = "controller";
    private const string _actionKey = "action";

    public string Controller { get => (string)this[_controllerKey]!; set => this[_controllerKey] = value; }
    public string Action { get => (string)this[_actionKey]!; set => this[_actionKey] = value; }

    public G4mvcActionRouteValues(string? area, string controller, string action) : base(area)
    {
        Controller = controller;
        Action = action;
    }

    /// <summary>
    /// Formats the route values into a <see cref="string"/> representation.
    /// Do not use this for navigation purposes! Use <see cref="ToString(IUrlHelper)"/> instead.
    /// </summary>
    /// <returns>The route formatted as [Area]/[Controller]/[Action]</returns>
    public override string ToString()
    {
#if !NETSTANDARD
        var length = 3 + (Area?.Length ?? -1) + Controller.Length + Action.Length; //3 is the maximum number of slashes, if area is null count -1

        return string.Create(length, this, (span, routeValues) =>
        {
            span[0] = '/';
            var currentIdx = 1;

            if (routeValues.Area is not null)
            {
                SpanPath.CopyPathSegmentToSpanAt(span, routeValues.Area, ref currentIdx);
            }

            SpanPath.CopyPathSegmentToSpanAt(span: span, routeValues.Controller, ref currentIdx);

            routeValues.Action.AsSpan().CopyTo(span[currentIdx..]);
        });
#else
        return $"/{(Area is null ? null : $"{Area}/")}{Controller}/{Action}";
#endif
    }
}
