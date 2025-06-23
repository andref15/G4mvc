namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : ClassDeclarationContext
{
    public string? Area { get; }
    public string Name { get; }
    public string NameWithoutSuffix { get; }

    private ControllerDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, syntax, typeSymbol, globalNullable)
    {
        Area = GetControllerArea(typeSymbol);
        Name = Syntax.Identifier.Text;
        NameWithoutSuffix = Name.RemoveEnd("Controller");
    }

    public static ControllerDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new ControllerDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!, ((CSharpCompilation)context.SemanticModel.Compilation).IsNullableEnabled());
    }

    private static string? GetControllerArea(INamedTypeSymbol typeSymbol)
    {
        var areaAttribute = typeSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass!.DerrivesFromType(TypeNames.AreaAttribute));

        return areaAttribute?.ConstructorArguments[0].Value as string;
    }
}
