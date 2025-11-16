namespace G4mvc.Generator.Contexts;

internal class MethodDeclarationContext(MethodDeclarationSyntax syntax, SemanticModel model, bool globalNullable) : BaseDeclarationContext(model, syntax.SpanStart, globalNullable)
{
    public MethodDeclarationSyntax Syntax { get; } = syntax;
    public IMethodSymbol MethodSymbol { get; } = model.GetDeclaredSymbol(syntax)!;
}
