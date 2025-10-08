namespace G4mvc.Generator.Contexts;
internal abstract class ClassDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol, bool globalNullable) : BaseDeclarationContext(model, syntax.SpanStart, globalNullable)
{
    public ClassDeclarationSyntax DeclarationNode { get; } = syntax;
    public INamedTypeSymbol TypeSymbol { get; } = typeSymbol;
}
