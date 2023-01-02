namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : BaseDeclarationContext
{
    public ClassDeclarationSyntax Syntax { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public string ControllerName { get; }
    public string ControllerNameWithoutSuffix { get; }

    public ControllerDeclarationContext(ClassDeclarationSyntax syntax, SemanticModel model) : base(model, syntax.SpanStart)
    {
        Syntax = syntax;
        TypeSymbol = model.GetDeclaredSymbol(syntax)!;

        ControllerName = Syntax.Identifier.Text;
        ControllerNameWithoutSuffix = ControllerName.RemoveEnd("Controller");
    }
}
