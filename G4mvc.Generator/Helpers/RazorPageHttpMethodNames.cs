namespace G4mvc.Generator.Helpers;
internal class RazorPageHttpMethodNames
{
    public const string Connect = "CONNECT";
    public const string Delete = "DELETE";
    public const string Get = "GET";
    public const string Head = "HEAD";
    public const string Options = "OPTIONS";
    public const string Patch = "PATCH";
    public const string Post = "POST";
    public const string Put = "PUT";
    public const string Trace = "TRACE";

    private static readonly string[] _methodNames =
    [
        Connect,
        Delete,
        Get,
        Head,
        Options,
        Patch,
        Post,
        Put,
        Trace
    ];

    private static readonly string[] _namePrefixes = ParseNamePrefixes();

    private static string[] ParseNamePrefixes()
    {
        var namePrefixes = new string[_methodNames.Length];
        for (var i = 0; i < namePrefixes.Length; i++)
        {
            namePrefixes[i] = $"On{_methodNames[i]}";
        }

        return namePrefixes;
    }

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

    public static (string Method, string? HandlerName) ParseMethodAndHandlerName(string handlerMethodName)
    {
        foreach (var methodName in _methodNames)
        {
            if (handlerMethodName.StartsWith(methodName, StringComparison.OrdinalIgnoreCase))
            {
                return (methodName.ToUpper(), handlerMethodName.Length != methodName.Length ? handlerMethodName.Substring(methodName.Length) : null);
            }
        }

        throw new ArgumentOutOfRangeException($"Method name '{handlerMethodName}' does not match any known HTTP method prefixes.");
    }
}
