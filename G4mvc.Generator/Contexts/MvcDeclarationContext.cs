namespace G4mvc.Generator.Contexts;

internal abstract class MvcDeclarationContext : ClassDeclarationContext
{
    public string? Area { get; }
    public string Name { get; }
    public string NameWithoutSuffix { get; }

    public MvcDeclarationContext(string suffix, SemanticModel model, ClassDeclarationSyntax declarationNode, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, declarationNode, typeSymbol, globalNullable)
    {
        Area = GetArea(typeSymbol);
        Name = declarationNode.Identifier.Text;
        NameWithoutSuffix = Name.RemoveEnd(suffix);
    }

    protected abstract string? GetArea(INamedTypeSymbol typeSymbol);

}
