namespace G4mvc.Generator.Helpers;
internal class RazorPageHttpMethodNames
{
    private static readonly string[] _namePrefixes =
    [
        "OnConnect",
        "OnDelete",
        "OnGet",
        "OnHead",
        "OnOptions",
        "OnPatch",
        "OnPost",
        "OnPut",
        "OnTrace",
    ];

    public static bool IsMatch(string methodName)
    {
        foreach (var prefix in _namePrefixes)
        {
            if (methodName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
