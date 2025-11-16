namespace G4mvc.Generator.Helpers;

internal static class GeneratorExecutionContextExtensions
{
    extension(SourceProductionContext context)
    {
        internal void AddGeneratedSource(string className, SourceBuilder source)
            => context.AddSource($"{className}.generated.cs", source.ToSourceText());
    }
}
