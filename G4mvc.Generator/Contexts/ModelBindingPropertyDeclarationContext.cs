namespace G4mvc.Generator.Contexts;

internal class ModelBindingPropertyDeclarationContext(PropertyDeclarationSyntax syntax, SemanticModel model, IPropertySymbol symbol, bool globalNullable) : PropertyDeclarationContext(syntax, model, symbol, globalNullable)
{
    public AttributeData? BindPropertyAttribute { get; } = symbol!.GetAttributes().FirstOrDefault(static a => a.AttributeClass!.ToDisplayString() == TypeNames.BindPropertyAttribute.FullName);
}
