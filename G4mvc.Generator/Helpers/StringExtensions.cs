namespace G4mvc.Generator.Helpers;
internal static class StringExtensions
{
    internal static string RemoveStart(this string @string, string value, StringComparison stringComparison = StringComparison.Ordinal)
        => @string.StartsWith(value, stringComparison) ? @string.Remove(0, value.Length) : @string;

    internal static string RemoveEnd(this string @string, string value, StringComparison stringComparison = StringComparison.Ordinal)
        => @string.EndsWith(value, stringComparison) ? @string.Remove(@string.Length - value.Length) : @string;

    internal static string Remove(this string @string, string start, string end, StringComparison stringComparison = StringComparison.Ordinal)
    {
        var span = @string.AsSpan();
        var startSpan = start.AsSpan();
        var endSpan = end.AsSpan();

        if (span.StartsWith(startSpan, stringComparison))
        {
            span = span.Slice(start.Length);
        }

        if (span.EndsWith(endSpan, stringComparison))
        {
            span = span.Slice(0, span.Length - endSpan.Length);
        }

        return span.ToString();
    }

    internal static string IfNotNullNullOrEmpty(this string? @string, string @if)
        => IfNotNullNullOrEmpty(@string, @if, string.Empty);

    internal static string IfNotNullNullOrEmpty(this string? @string, string @if, string @else)
        => !string.IsNullOrEmpty(@string) ? @if : @else;

    internal static string FirstCharLower(this string @string)
    {
        Span<char> span = stackalloc char[@string.Length];

        span[0] = char.ToLower(@string[0]);

        for (var i = 1; i < @string.Length; i++)
        {
            span[i] = @string[i];
        }

        return span.ToString();
    }
}
