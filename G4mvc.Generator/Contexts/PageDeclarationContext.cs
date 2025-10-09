namespace G4mvc.Generator.Contexts;
internal class PageDeclarationContext : MvcDeclarationContext
{
    public const string Suffix = "Model";

    private PageDeclarationContext(SemanticModel model, ClassDeclarationSyntax syntax, INamedTypeSymbol typeSymbol, bool globalNullable) : base(Suffix, model, syntax, typeSymbol, globalNullable)
    {
    }

    public static PageDeclarationContext Create(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        return new PageDeclarationContext(context.SemanticModel, classDeclaration, context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken)!, ((CSharpCompilation)context.SemanticModel.Compilation).IsNullableEnabled());
    }

    protected override string? GetArea(INamedTypeSymbol typeSymbol)
    {
        var typeNamespace = typeSymbol.ContainingNamespace;
        var areasNamespace = GetAreasNamespace(typeNamespace);

        if (areasNamespace is null)
        {
            return null;
        }

        var remainingNs = typeNamespace.ToDisplayString().AsSpan().Slice(areasNamespace.ToDisplayString().Length);
        var areaEndIdx = remainingNs.IndexOf('.');

        return areaEndIdx is -1
            ? remainingNs.ToString()
            : remainingNs.Slice(0, areaEndIdx).ToString();
    }

    private static INamespaceSymbol? GetAreasNamespace(INamespaceSymbol symbol)
    {
        var current = symbol;

        for (var i = 0; i < 64; i++)
        {
            var containing = current.ContainingNamespace;

            if (containing is null)
            {
                break;
            }

            if (containing.Name == "Areas")
            {
                return current;
            }

            current = containing;
        }

        return null;
    }
}
