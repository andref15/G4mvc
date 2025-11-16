#if NETCOREAPP

namespace G4mvc.Helpers;

internal static class SpanPath
{
    public static void CopyPathSegmentToSpanAt(Span<char> span, ReadOnlySpan<char> value, ref int idx)
    {
        value.CopyTo(span[idx..]);
        idx += value.Length;
        span[idx++] = '/';
    }
}

#endif