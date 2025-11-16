using System.Reflection;

namespace G4mvc.Generator.Helpers;

internal static class GeneratorInfos
{
    internal static string GeneratorName { get; } = typeof(G4mvcGenerator).FullName;
    internal static string GeneratorVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
}
