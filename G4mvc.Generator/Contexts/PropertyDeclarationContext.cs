namespace G4mvc.Generator.Contexts;
internal class PropertyDeclarationContext(PropertyDeclarationSyntax syntax, SemanticModel model, bool globalNullable) : BaseDeclarationContext(model, syntax.SpanStart, globalNullable)
{
    public PropertyDeclarationSyntax Syntax { get; } = syntax;
    public IPropertySymbol PropertySymbol { get; } = model.GetDeclaredSymbol(syntax)!;
}
