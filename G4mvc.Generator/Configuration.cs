using System.Text.Json;

namespace G4mvc.Generator;
internal partial class Configuration
{
    public const string FileName = "G4mvc.json";
    public const string RoutesNameSpace = $"{nameof(G4mvc)}.Routes";

    public LanguageVersion LanguageVersion { get; private set; }
    public JsonConfigClass JsonConfig { get; private set; } = null!;
    public List<string> GlobalUsings { get; private set; } = new();
    public bool GlobalNullable { get; private set; }
    public string GeneratedClassModifier => JsonConfig.MakeGeneratedClassesInternal ? "internal" : "public";

    internal static Configuration CreateConfig(CSharpCompilation compilation, string? configFile)
    {
        Configuration configuration = new()
        {
            LanguageVersion = compilation.LanguageVersion,
            GlobalNullable = compilation.IsNullableEnabled(),
            JsonConfig = configFile is null ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile) ?? new()
        };

        configuration.JsonConfig.SetDefaults();

        return configuration;
    }

    internal static Configuration CreateConfig(CSharpParseOptions parseOptions, string? configFile)
    {
        Configuration configuration = new()
        {
            LanguageVersion = parseOptions.LanguageVersion,
            GlobalNullable = true,
            JsonConfig = configFile is null ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile) ?? new()
        };

        configuration.JsonConfig.SetDefaults();

        return configuration;
    }

    internal SourceBuilder CreateSourceBuilder()
        => new(LanguageVersion);

    internal class JsonConfigClass
    {
        public string HelperClassName { get; set; } = null!;
        public string LinksClassName { get; set; } = null!;
        public string StaticFilesPath { get; set; } = null!;
        public bool UseVirtualPathProcessor { get; set; }
        public bool MakeGeneratedClassesInternal { get; set; }
        public List<string>? ExcludedStaticFileExtensions { get; set; }
        public List<string>? ExcludedStaticFileDirectories { get; set; }
        public Dictionary<string, string>? AdditionalStaticFilesPaths { get; set; }
        public Dictionary<string, string>? CustomStaticFileDirectoryAlias { get; set; }

        internal void SetDefaults()
        {
            HelperClassName ??= "MVC";
            LinksClassName ??= "Links";
            StaticFilesPath ??= "wwwroot";
        }
    }
}
