using System.Text.Json;

namespace G4mvc.Generator;
internal partial class Configuration
{
    public const string FileName = "G4mvc.json";

    public LanguageVersion LanguageVersion { get; private set; }
    public JsonConfigClass JsonConfig { get; private set; } = null!;
    public List<string> GlobalUsings { get; private set; } = new();
    public bool GlobalNullable { get; private set; }

    public static Configuration Instance { get; private set; } = null!;

    internal static void CreateConfig(CSharpCompilation compilation, string? configFile)
    {
        Instance = new()
        {
            LanguageVersion = compilation.LanguageVersion
        };

        if (Instance.LanguageVersion >= LanguageVersion.CSharp10)
        {
            Instance.GlobalUsings = compilation.SyntaxTrees.SelectMany(st => st.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Where(ud => ud.GlobalKeyword.Text is "global").Select(ud => ud.Name.ToString().RemoveStart("global::")).ToList();
        }

        Instance.GlobalNullable = compilation.Options.NullableContextOptions is not NullableContextOptions.Disable;

        Instance.JsonConfig = configFile is null ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile) ?? new();

        Instance.JsonConfig.SetDefaults();
    }

    internal SourceBuilder CreateSourceBuilder()
        => new(LanguageVersion, GlobalUsings);

    internal class JsonConfigClass
    {
        public string HelperClassName { get; set; } = null!;
        public string LinksClassName { get; set; } = null!;
        public string StaticFilesPath { get; set; } = null!;
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
