namespace G4mvc.Generator.Contexts;
internal class ControllerDeclarationContext : BaseDeclarationContext
{
    public ClassDeclarationSyntax Syntax { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public string? ControllerArea { get; }
    public string ControllerName { get; }
    public string ControllerNameWithoutSuffix { get; }

    private ControllerDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, syntax.SpanStart, globalNullable)
    {
        Syntax = syntax;
        TypeSymbol = typeSymbol;

        ControllerArea = GetControllerArea(typeSymbol);
        ControllerName = Syntax.Identifier.Text;
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
