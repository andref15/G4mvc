namespace G4mvc.TestBase.Utils;

public static class EnumerableUtils
{
    public static IEnumerable<T> Create<T>(T item)
    {
        yield return item;
    }
}
