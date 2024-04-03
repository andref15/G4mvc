using System.Text.Json;
using System.Text.Json.Serialization;

namespace G4mvc.Generator;
internal struct Configuration(LanguageVersion languageVersion, bool globalNullable, string? configFile, AnalyzerConfigValues analyzerConfigValues)
{
    private enum ClassNamespaceIdentifier
    {
        global,
        project
    }

    public const string FileName = "g4mvc.json";
    public const string RoutesNameSpace = $"{nameof(G4mvc)}.Routes";
    private string? _generatedClassNamespace;
    private string? _generatedClassModifier;

    public LanguageVersion LanguageVersion { get; } = languageVersion;
    public AnalyzerConfigValues AnalyzerConfigValues { get; } = analyzerConfigValues;
    public JsonConfigClass JsonConfig { get; } = string.IsNullOrWhiteSpace(configFile) ? new() : JsonSerializer.Deserialize<JsonConfigClass>(configFile!);
    public bool GlobalNullable { get; } = globalNullable;
    public string? GeneratedClassNamespace
    {
        get
        {
            if (_generatedClassNamespace is not null)
            {
                return _generatedClassNamespace;
            }

            var @namespace = JsonConfig.GeneratedClassNamespace;

            if (Enum.TryParse<ClassNamespaceIdentifier>(@namespace, true, out var identifier))
            {
                @namespace = identifier switch
                {
                    ClassNamespaceIdentifier.global => null,
                    ClassNamespaceIdentifier.project => AnalyzerConfigValues.RootNamespace,
                    _ => null
                };
            }

            return _generatedClassNamespace = @namespace;
        }
    }

    public string GeneratedClassModifier => _generatedClassModifier ??= JsonConfig.MakeGeneratedClassesInternal ? "internal" : "public";

    internal static Configuration CreateConfig(CSharpCompilation compilation, string? configFile, AnalyzerConfigValues analyzerConfigValues)
        => new(compilation.LanguageVersion, compilation.IsNullableEnabled(), configFile, analyzerConfigValues);

    internal static Configuration CreateConfig(CSharpParseOptions parseOptions, string? configFile, AnalyzerConfigValues analyzerConfigValues)
        => new(parseOptions.LanguageVersion, true, configFile, analyzerConfigValues);

    internal readonly SourceBuilder CreateSourceBuilder()
        => new(LanguageVersion);

    internal readonly struct JsonConfigClass
    {
        public string HelperClassName { get; }
        public string LinksClassName { get; }
        public string StaticFilesPath { get; }
        public bool UseVirtualPathProcessor { get; }
        public bool UseProcessedPathForContentLink { get; }
        public bool MakeGeneratedClassesInternal { get; }
        public string GeneratedClassNamespace { get; }
        public string[]? ExcludedStaticFileExtensions { get; }
        public string[]? ExcludedStaticFileDirectories { get; }
        public IReadOnlyDictionary<string, string>? AdditionalStaticFilesPaths { get; }
        public IReadOnlyDictionary<string, string>? CustomStaticFileDirectoryAlias { get; }

        public JsonConfigClass()
        {
            HelperClassName = "MVC";
            LinksClassName = "Links";
            StaticFilesPath = "wwwroot";
            GeneratedClassNamespace = "global";
        }

        [JsonConstructor]
        public JsonConfigClass(string? helperClassName, string? linksClassName, string? staticFilesPath, bool useVirtualPathProcessor, bool useProcessedPathForContentLink, bool makeGeneratedClassesInternal, string? generatedClassNamespace, string[]? excludedStaticFileExtensions, string[]? excludedStaticFileDirectories, IReadOnlyDictionary<string, string>? additionalStaticFilesPaths, IReadOnlyDictionary<string, string>? customStaticFileDirectoryAlias)
        {
            HelperClassName = helperClassName ?? "MVC";
            LinksClassName = linksClassName ?? "Links";
            StaticFilesPath = staticFilesPath ?? "wwwroot";
            UseVirtualPathProcessor = useVirtualPathProcessor;
            UseProcessedPathForContentLink = useProcessedPathForContentLink;
            MakeGeneratedClassesInternal = makeGeneratedClassesInternal;
            GeneratedClassNamespace = generatedClassNamespace ?? nameof(ClassNamespaceIdentifier.global);
            ExcludedStaticFileExtensions = excludedStaticFileExtensions;
            ExcludedStaticFileDirectories = excludedStaticFileDirectories;
            AdditionalStaticFilesPaths = additionalStaticFilesPaths;
            CustomStaticFileDirectoryAlias = customStaticFileDirectoryAlias;
        }
    }
}
