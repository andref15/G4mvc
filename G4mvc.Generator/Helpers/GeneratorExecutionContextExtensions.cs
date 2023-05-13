namespace G4mvc.Generator.Helpers;
internal static class GeneratorExecutionContextExtensions
{
    internal static void AddGeneratedSource(this SourceProductionContext context, string className, SourceBuilder source)
        => context.AddSource($"{className}.generated.cs", source.ToSourceText());
}
