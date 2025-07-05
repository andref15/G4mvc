namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : BaseDeclarationContext
{
    public ClassDeclarationSyntax DeclarationNode { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public string? ControllerArea { get; }
    public string ControllerName { get; }
    public string ControllerNameWithoutSuffix { get; }

    private ControllerDeclarationContext(SemanticModel model, ClassDeclarationSyntax declarationNode, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, declarationNode.SpanStart, globalNullable)
    {
        DeclarationNode = declarationNode;
        TypeSymbol = typeSymbol;

        ControllerArea = GetControllerArea(typeSymbol);
        ControllerName = declarationNode.Identifier.Text;
        ControllerNameWithoutSuffix = ControllerName.RemoveEnd("Controller");
    }

    public static ControllerDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new ControllerDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!, ((CSharpCompilation)context.SemanticModel.Compilation).IsNullableEnabled());
    }

    private static string? GetControllerArea(INamedTypeSymbol typeSymbol)
    {
        var areaAttribute = typeSymbol.GetAttributes(true).FirstOrDefault(a => a.AttributeClass!.DerrivesFromType(TypeNames.AreaAttribute));

        return areaAttribute?.ConstructorArguments[0].Value as string;
    }
}
