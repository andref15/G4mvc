namespace G4mvc.Generator.Helpers;

internal static class TypeSymbolExtensions
{
    extension(ITypeSymbol typeSymbol)
    {
        internal bool IsTypeOrInterface(string type)
            => typeSymbol.ToDisplayString() == type;

        internal bool DerrivesFromType(string type)
            => IsTypeOrInterface(typeSymbol, type) || (typeSymbol.BaseType is not null && DerrivesFromType(typeSymbol.BaseType, type));

        internal bool IsOrImplementsInterface(string @interface)
            => typeSymbol.ToDisplayString() == @interface || typeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == @interface);
    }

    extension(INamedTypeSymbol typeSymbol)
    {
        public IEnumerable<AttributeData> GetAttributes(bool includeBaseTypes = true)
        {
            foreach (var attribute in typeSymbol.GetAttributes())
            {
                yield return attribute;
            }

            if (includeBaseTypes && typeSymbol.BaseType is not null)
            {
                foreach (var baseAttribute in GetAttributes(typeSymbol.BaseType, true))
                {
                    yield return baseAttribute;
                }
            }
        }
    }
}
