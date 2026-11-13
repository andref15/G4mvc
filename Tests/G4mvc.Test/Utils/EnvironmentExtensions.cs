namespace G4mvc.Test.Utils;

internal static class EnvironmentVersion
{
    public static string VersionString => field ??= Environment.Version.ToString(3);
    public static string NetVersion => field ??= $"net{Environment.Version.Major}.{Environment.Version.Minor}";
}
