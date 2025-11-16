namespace G4mvc.Generator.CSharp;

internal static class SourceCode
{
    public const string NewCtor = "new()";

    public static string String(object? value)
        => value is null ? "null" : value is "" ? "string.Empty" : $"\"{value}\"";

    public static string Nameof(string variable)
        => $"nameof({variable})";

    public static string? GetDefaultValue(ParameterSyntax syntax)
        => syntax.Default is null ? null : $" {syntax.Default}";

}
