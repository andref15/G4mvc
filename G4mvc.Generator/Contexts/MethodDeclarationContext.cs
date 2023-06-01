namespace G4mvc.Generator.Contexts;
internal class MethodDeclarationContext : BaseDeclarationContext
{
    public MethodDeclarationSyntax Syntax { get; }
    public IMethodSymbol MethodSymbol { get; }

    public MethodDeclarationContext(MethodDeclarationSyntax syntax, SemanticModel model, bool globalNullable) : base(model, syntax.SpanStart, globalNullable)
    {
        Syntax = syntax;
        MethodSymbol = model.GetDeclaredSymbol(syntax)!;
    }
}
