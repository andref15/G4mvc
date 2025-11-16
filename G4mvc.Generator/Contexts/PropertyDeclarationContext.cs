using System.Collections.Immutable;

namespace G4mvc.Generator.Contexts;

internal class PropertyDeclarationContext : BaseDeclarationContext
{
    public string Name { get; }
    public PropertyDeclarationSyntax Syntax { get; }
    public IPropertySymbol PropertySymbol { get; }
    public ImmutableArray<AttributeData> Attributes { get; }

    public PropertyDeclarationContext(PropertyDeclarationSyntax syntax, SemanticModel model, bool globalNullable) : base(model, syntax.SpanStart, globalNullable)
    {
        Name = syntax.Identifier.Text;
        Syntax = syntax;
        var symbol = model.GetDeclaredSymbol(syntax)!;
        PropertySymbol = symbol;
        Attributes = symbol.GetAttributes();
    }

    public PropertyDeclarationContext(PropertyDeclarationSyntax syntax, SemanticModel model, IPropertySymbol symbol, bool globalNullable) : base(model, syntax.SpanStart, globalNullable)
    {
        Name = syntax.Identifier.Text;
        Syntax = syntax;
        PropertySymbol = symbol;
        Attributes = symbol.GetAttributes();
    }
}
