#if !NETSTANDARD
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#endif

namespace G4mvc;

public abstract class G4mvcBaseRouteValues<TSelf>
#if NETSTANDARD
        : Dictionary<string, object?>
#else
        : RouteValueDictionary
#endif
    where TSelf : G4mvcBaseRouteValues<TSelf>
{
    private const string _areaKey = "area";

    public string? Area { get => (string?)this[_areaKey]; set => this[_areaKey] = value; }

    public G4mvcBaseRouteValues(string? area)
    {
        Area = area;
    }

#if !NETSTANDARD
    public TSelf WithValue(string key, object? value)
    {
        ThrowIfKeyIsNullOrEmpty(key);

        this[key] = value;

        return (TSelf)this;
    }

    public TSelf WithValues(params IEnumerable<KeyValuePair<string, object?>> keyValuePairs)
    {
        foreach (var (key, value) in keyValuePairs)
        {
            ThrowIfKeyIsNullOrEmpty(key, nameof(keyValuePairs));
            this[key] = value;
        }

        return (TSelf)this;

    }

    public TSelf WithoutValue(string key)
    {
        ThrowIfKeyIsNullOrEmpty(key);

        Remove(key);

        return (TSelf)this;
    }

    public TSelf WithoutValues(params IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            ThrowIfKeyIsNullOrEmpty(key, nameof(keys));

            Remove(key);
        }

        return (TSelf)this;
    }

    public RouteValueDictionary AsRouteValueDictionary()
        => this;

    public string? ToString(IUrlHelper urlHelper)
        => urlHelper.RouteUrl(this);

    private static void ThrowIfKeyIsNullOrEmpty(string key, [CallerArgumentExpression(nameof(key))] string paramName = "")
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            ThrowKeyInvalidException(paramName);
        }
    }

    [DoesNotReturn]
    private static void ThrowKeyInvalidException(string paramName)
        => throw new ArgumentException("Keys cannot be an empty string or composed entirely of whitespace.", paramName);
#endif
}
