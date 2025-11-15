namespace G4mvc.Generator.Helpers;

internal static class KeyValuePair
{
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        => new(key, value);

    extension<TKey, TValue>(KeyValuePair<TKey, TValue> kvp)
    {
        public void Deconstruct(out TKey key, out TValue value)
            => (key, value) = (kvp.Key, kvp.Value);
    }
}
