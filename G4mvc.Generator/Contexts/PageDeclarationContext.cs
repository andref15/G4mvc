namespace G4mvc.Generator.Contexts;
internal class PageDeclarationContext : ClassDeclarationContext
{
    public const string Suffix = "Page";

    public string? Area { get; }
    public string Name { get; }

    private PageDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, syntax, typeSymbol, globalNullable)
    {
        Area = GetPageArea(typeSymbol);
        Name = DeclarationNode.Identifier.Text;
    }

    public static PageDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new PageDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!, ((CSharpCompilation)context.SemanticModel.Compilation).IsNullableEnabled());
    }

    private static string? GetPageArea(INamedTypeSymbol typeSymbol)
    {
        _ = typeSymbol;
        return null; // TODO
    }
}
