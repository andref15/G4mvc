namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : BaseDeclarationContext
{
    public ClassDeclarationSyntax Syntax { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public string ControllerName { get; }
    public string ControllerNameWithoutSuffix { get; }

    private ControllerDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol) : base(model, syntax.SpanStart)
    {
        Syntax = syntax;
        TypeSymbol = typeSymbol;

        ControllerName = Syntax.Identifier.Text;
        ControllerNameWithoutSuffix = ControllerName.RemoveEnd("Controller");
    }

    public static ControllerDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new ControllerDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!);
    }
}
