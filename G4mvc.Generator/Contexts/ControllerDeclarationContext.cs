namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : ClassDeclarationContext
{
    public const string Suffix = "Controller";

    public string? Area { get; }
    public string Name { get; }
    public string NameWithoutSuffix { get; }

    private ControllerDeclarationContext(SemanticModel model, ClassDeclarationSyntax declarationNode, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, declarationNode, typeSymbol, globalNullable)
    {
        Area = GetControllerArea(typeSymbol);
        Name = declarationNode.Identifier.Text;
        NameWithoutSuffix = Name.RemoveEnd(Suffix);
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
