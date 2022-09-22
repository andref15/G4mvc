namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext
{
    public ClassDeclarationSyntax Syntax { get; }
    public SemanticModel Model { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public bool NullableEnabled { get; set; }
    public string ControllerName { get; }
    public string ControllerNameWithoutSuffix { get; }

    public ControllerDeclarationContext(ClassDeclarationSyntax syntax, SemanticModel model)
    {
        Syntax = syntax;
        Model = model;
        TypeSymbol = model.GetDeclaredSymbol(syntax)!;
        NullableEnabled = model.GetNullableContext(syntax.SpanStart) != NullableContext.Disabled;

        ControllerName = Syntax.Identifier.Text;
        ControllerNameWithoutSuffix = ControllerName.RemoveEnd("Controller");
    }
}
