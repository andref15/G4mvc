namespace G4mvc.Generator.Contexts;
internal class PagesDeclarationContext : ClassDeclarationContext
{
    public string? Area { get; }
    public string Name { get; }

    private PagesDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol, bool globalNullable) : base(model, syntax, typeSymbol, globalNullable)
    {
        Area = GetPageArea(typeSymbol);
        Name = Syntax.Identifier.Text;
    }

    public static PagesDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new PagesDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!, ((CSharpCompilation)context.SemanticModel.Compilation).IsNullableEnabled());
    }

    private static string? GetPageArea(INamedTypeSymbol typeSymbol)
    {
        _ = typeSymbol;
        return null; // TODO
    }
}
