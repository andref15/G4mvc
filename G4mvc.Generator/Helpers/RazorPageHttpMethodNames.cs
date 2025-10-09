namespace G4mvc.Generator.Helpers;
internal class RazorPageHttpMethodNames
{
    private static readonly string[] _httpMethods =
    [
        "Connect",
        "Delete",
        "Get",
        "Head",
        "Options",
        "Patch",
        "Post",
        "Put",
        "Trace",
    ];

    private static readonly HashSet<string> _names = ParseHttpMethodNames();

    public static HashSet<string> NameSet => _names;

    private static HashSet<string> ParseHttpMethodNames()
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var method in _httpMethods)
        {
            names.Add($"On{method}");
            names.Add($"On{method}Async");
        }

        return names;
    }
}
