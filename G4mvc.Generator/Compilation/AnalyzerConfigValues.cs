using Microsoft.CodeAnalysis.Diagnostics;

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

    public static AnalyzerConfigValues FromAnalyzerConfigOptions(AnalyzerConfigOptions analyzerConfigOptions)
    {
        if (!analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.ProjectDir, out var projectDir) || string.IsNullOrWhiteSpace(projectDir))
        {
            throw new InvalidOperationException($"No AnalyzerConfigOption for {GlobalOptionConstant.BuildProperty.ProjectDir} could be found! This should not happen.");
        }

        _ = analyzerConfigOptions.TryGetValue(GlobalOptionConstant.BuildProperty.RootNamespace, out var rootNamespace);

        return new AnalyzerConfigValues(projectDir, rootNamespace);
    }
}
