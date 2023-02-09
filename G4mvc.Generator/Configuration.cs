using System.Text.Json;

namespace G4mvc.Generator;
internal partial class Configuration
{
    public LanguageVersion LanguageVersion { get; private set; }
    public JsonConfigClass JsonConfig { get; private set; } = null!;
    public List<string> GlobalUsings { get; private set; } = new();
    public bool GlobalNullable { get; private set; }

    public static Configuration Instance { get; private set; } = null!;

    internal static void CreateConfig(GeneratorExecutionContext context)
    {
        CSharpCompilation compilation = (context.Compilation as CSharpCompilation)!;

        Instance = new()
        {
            LanguageVersion = compilation.LanguageVersion
        };

        if (Instance.LanguageVersion >= LanguageVersion.CSharp10)
        {
            Instance.GlobalUsings = context.Compilation.SyntaxTrees.SelectMany(st => st.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Where(ud => ud.GlobalKeyword.Text is "global").Select(ud => ud.Name.ToString().RemoveStart("global::")).ToList();
        }

        Instance.GlobalNullable = compilation.Options.NullableContextOptions is not NullableContextOptions.Disable;

        AdditionalText? configFile = context.AdditionalFiles.FirstOrDefault(f => Path.GetFileName(f.Path).Equals("g4mvc.json", StringComparison.OrdinalIgnoreCase));
        Instance.JsonConfig = configFile is null ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile.GetText(context.CancellationToken)!.ToString()) ?? new();

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

        internal void SetDefaults()
        {
            HelperClassName ??= "MVC";
            LinksClassName ??= "Links";
            StaticFilesPath ??= "wwwroot";
        }
    }
}
