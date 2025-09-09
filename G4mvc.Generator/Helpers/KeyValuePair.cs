namespace G4mvc.Generator.Helpers;
internal static class KeyValuePair
{
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        => new(key, value);
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
        => (key, value) = (pair.Key, pair.Value);
}
