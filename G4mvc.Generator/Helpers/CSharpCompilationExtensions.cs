namespace G4mvc.Generator.Helpers;

internal static class CSharpCompilationExtensions
{
    extension(CSharpCompilation compilation)
    {
        public bool IsNullableEnabled()
        {
            return compilation.Options.NullableContextOptions is not NullableContextOptions.Disable;
        }
    }
}
