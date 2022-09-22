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
}
