namespace G4mvc.Generator.Compilation;
internal readonly struct AnalyzerConfigValues
{
    public readonly string ProjectDir { get; }
    public readonly string? RootNamespace { get; }

    public AnalyzerConfigValues()
    {
        ProjectDir = null!;
        RootNamespace = null;
    }

    public AnalyzerConfigValues(string projectDir, string? rootNamespace)
    {
        ProjectDir = projectDir;
        RootNamespace = rootNamespace;
    }
}
