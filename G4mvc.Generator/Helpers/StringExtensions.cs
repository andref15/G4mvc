namespace G4mvc.Generator.Helpers;
internal static class StringExtensions
{
    internal static string RemoveStart(this string @string, string value, StringComparison stringComparison = StringComparison.CurrentCulture)
        => @string.StartsWith(value, stringComparison) ? @string.Remove(0, value.Length) : @string;

    internal static string RemoveEnd(this string @string, string value, StringComparison stringComparison = StringComparison.CurrentCulture)
        => @string.EndsWith(value, stringComparison) ? @string.Remove(@string.Length - value.Length) : @string;
}
