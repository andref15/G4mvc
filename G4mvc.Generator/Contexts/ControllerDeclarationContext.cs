namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : MvcDeclarationContext
{
    public const string Suffix = "Controller";

    private ControllerDeclarationContext(SemanticModel model, ClassDeclarationSyntax declarationNode, INamedTypeSymbol typeSymbol, bool globalNullable) : base(Suffix, model, declarationNode, typeSymbol, globalNullable)
    {

    }

    public static ControllerDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new ControllerDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!, ((CSharpCompilation)context.SemanticModel.Compilation).IsNullableEnabled());
    }

    protected override string? GetArea(INamedTypeSymbol typeSymbol)
    {
        var areaAttribute = typeSymbol.GetAttributes(true).FirstOrDefault(a => a.AttributeClass!.DerrivesFromType(TypeNames.AreaAttribute));

        return areaAttribute?.ConstructorArguments[0].Value as string;
    }
}
