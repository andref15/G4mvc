#if NETCOREAPP
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
#endif

namespace G4mvc;

public abstract class G4mvcBaseRouteValues
#if NETCOREAPP
        : RouteValueDictionary
#else
        : Dictionary<string, object?>
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

#if NETCOREAPP
    public RouteValueDictionary AsRouteValueDictionary()
        => this;

    public string? ToString(IUrlHelper urlHelper)
        => urlHelper.RouteUrl(this);
#endif
}
