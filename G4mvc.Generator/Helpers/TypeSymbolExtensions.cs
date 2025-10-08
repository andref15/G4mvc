namespace G4mvc.Generator.Helpers;
internal static class TypeSymbolExtensions
{
    internal static bool IsTypeOrInterface(this ITypeSymbol typeSymbol, string type)
        => typeSymbol.ToDisplayString() == type;

    internal static bool DerrivesFromType(this ITypeSymbol typeSymbol, string type)
        => IsTypeOrInterface(typeSymbol, type) || (typeSymbol.BaseType is not null && DerrivesFromType(typeSymbol.BaseType, type));

    internal static bool IsOrImplementsInterface(this ITypeSymbol typeSymbol, string @interface)
        => typeSymbol.ToDisplayString() == @interface || typeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == @interface);

    public static IEnumerable<AttributeData> GetAttributes(this INamedTypeSymbol type, bool includeBaseTypes = true)
    {
        foreach (var attribute in type.GetAttributes())
        {
            yield return attribute;
        }

        if (includeBaseTypes && type.BaseType is not null)
        {
            foreach (var baseAttribute in GetAttributes(type.BaseType, true))
            {
                yield return baseAttribute;
            }
        }
    }
}
