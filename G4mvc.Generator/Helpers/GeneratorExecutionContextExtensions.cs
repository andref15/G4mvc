namespace G4mvc.Generator.Helpers;
internal static class GeneratorExecutionContextExtensions
{
    internal static void AddGeneratedSource(this GeneratorExecutionContext context, string className, SourceBuilder source)
        => context.AddSource($"{className}.generated.cs", source.ToSourceText());
}
