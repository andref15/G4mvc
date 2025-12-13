#if !NETSTANDARD
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
#endif

namespace G4mvc;

public abstract class G4mvcBaseRouteValues
#if NETSTANDARD
        : Dictionary<string, object?>
#else
        : RouteValueDictionary
#endif
{
    private const string _areaKey = "area";
    private const string _controllerKey = "controller";
    private const string _actionKey = "action";

    public string? Area { get => (string?)this[_areaKey]; set => this[_areaKey] = value; }

    public G4mvcBaseRouteValues(string? area)
    {
        Area = area;
    }

#if !NETSTANDARD
    public RouteValueDictionary AsRouteValueDictionary()
        => this;

    public string? ToString(IUrlHelper urlHelper)
        => urlHelper.RouteUrl(this);
#endif
}
