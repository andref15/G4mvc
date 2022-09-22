namespace G4mvc.Generator.Contexts;
internal class MethodDeclarationContext
{
    public MethodDeclarationSyntax Syntax { get; }
    public IMethodSymbol MethodSymbol { get; }

    public MethodDeclarationContext(MethodDeclarationSyntax syntax, IMethodSymbol methodSymbol)
    {
        Syntax = syntax;
        MethodSymbol = methodSymbol;
    }
}
