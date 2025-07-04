using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G4mvc.Generator.Compilation;
class MyClass
{

}
internal struct Configuration(LanguageVersion languageVersion, bool globalNullable, string? configFile, AnalyzerConfigValues analyzerConfigValues)
{
    private enum ClassNamespaceIdentifier
    {
        Global,
        Project
    }

    public const string FileName = "g4mvc.json";
    public const string RoutesNameSpace = $"{nameof(G4mvc)}.Routes";
    public const string AreasNameSpace = $"{nameof(G4mvc)}.Areas";
    public const string VppClassName = "VirtualPathProcessor";
    public const string VppMethodName = "Process";
    private string? _generatedClassNamespace;
    private bool _generatedClassNamespaceInitialized = false;
    private string? _generatedClassModifier;

    public LanguageVersion LanguageVersion { get; } = languageVersion;
    public AnalyzerConfigValues AnalyzerConfigValues { get; } = analyzerConfigValues;
    public JsonConfigModel JsonConfig { get; } = string.IsNullOrWhiteSpace(configFile) ? new() : JsonSerializer.Deserialize<JsonConfigModel>(configFile!);
    public bool GlobalNullable { get; } = globalNullable;
    public string? GeneratedClassNamespace
    {
        get
        {
            if (_generatedClassNamespaceInitialized)
            {
                return _generatedClassNamespace;
            }

            var @namespace = JsonConfig.GeneratedClassNamespace;

            if (@namespace[0] is '.')
            {
                @namespace = $"{AnalyzerConfigValues.RootNamespace}{@namespace}";
            }
            else if (Enum.TryParse<ClassNamespaceIdentifier>(@namespace, true, out var identifier))
            {
                @namespace = identifier switch
                {
                    ClassNamespaceIdentifier.Global => null,
                    ClassNamespaceIdentifier.Project => AnalyzerConfigValues.RootNamespace,
                    _ => null
                };
            }

            _generatedClassNamespaceInitialized = true;

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

    internal readonly struct JsonConfigModel
    {

        private const string _defaultMvcHelperClassName = "MVC";
        private const string _defaultPageHelperClassName = "Pages";
        private const string _defaultLinksHelperClassName = "Links";
        private const string _defaultStaticFilesPath = "wwwroot";
        private const string _defaultGeneratedClassNamespace = "global";

        public string MvcHelperClassName { get; }
        public string PageHelperClassName { get; }
        public string LinksHelperClassName { get; }
        public string StaticFilesPath { get; }
        public bool UseVirtualPathProcessor { get; }

        [JsonInclude, JsonPropertyName(nameof(UseProcessedPathForContentLink))]
        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for json")]
        private bool? UseProcessedPathForContentLinkNullable { get; }

        [JsonIgnore]
        public bool UseProcessedPathForContentLink { get; }
        public bool MakeGeneratedClassesInternal { get; }
        public string GeneratedClassNamespace { get; }
        public bool EnableSubfoldersInViews { get; }
        public string[]? ExcludedStaticFileExtensions { get; }
        public string[]? ExcludedStaticFileDirectories { get; }
        public IReadOnlyDictionary<string, string>? AdditionalStaticFilesPaths { get; }
        public IReadOnlyDictionary<string, string>? CustomStaticFileDirectoryAlias { get; }

        public JsonConfigModel()
        {
            MvcHelperClassName = _defaultMvcHelperClassName;
            PageHelperClassName = _defaultPageHelperClassName;
            LinksHelperClassName = _defaultLinksHelperClassName;
            StaticFilesPath = _defaultStaticFilesPath;
            GeneratedClassNamespace = _defaultGeneratedClassNamespace;
        }

        [JsonConstructor]
        public JsonConfigModel(string? mvcHelperClassName, string? pageHelperClassName, string? linksHelperClassName, string? staticFilesPath, bool useVirtualPathProcessor, bool? useProcessedPathForContentLinkNullable, bool makeGeneratedClassesInternal, string? generatedClassNamespace, bool enableSubfoldersInViews, string[]? excludedStaticFileExtensions, string[]? excludedStaticFileDirectories, IReadOnlyDictionary<string, string>? additionalStaticFilesPaths, IReadOnlyDictionary<string, string>? customStaticFileDirectoryAlias)
        {
            MvcHelperClassName = string.IsNullOrWhiteSpace(mvcHelperClassName)
                ? _defaultMvcHelperClassName
                : mvcHelperClassName!.Trim();
            PageHelperClassName = string.IsNullOrWhiteSpace(pageHelperClassName)
                ? _defaultPageHelperClassName
                : pageHelperClassName!.Trim();
            LinksHelperClassName = string.IsNullOrWhiteSpace(linksHelperClassName)
                ? _defaultLinksHelperClassName
                : linksHelperClassName!.Trim();
            StaticFilesPath = string.IsNullOrWhiteSpace(staticFilesPath)
                ? _defaultStaticFilesPath
                : staticFilesPath!.Trim();
            UseVirtualPathProcessor = useVirtualPathProcessor;
            UseProcessedPathForContentLinkNullable = useProcessedPathForContentLinkNullable;
            UseProcessedPathForContentLink = useVirtualPathProcessor && (useProcessedPathForContentLinkNullable ?? true);
            MakeGeneratedClassesInternal = makeGeneratedClassesInternal;
            GeneratedClassNamespace = string.IsNullOrWhiteSpace(generatedClassNamespace)
                ? nameof(ClassNamespaceIdentifier.Project)
                : generatedClassNamespace!.Trim();
            EnableSubfoldersInViews = enableSubfoldersInViews;
            ExcludedStaticFileExtensions = excludedStaticFileExtensions;
            ExcludedStaticFileDirectories = excludedStaticFileDirectories;
            AdditionalStaticFilesPaths = additionalStaticFilesPaths;
            CustomStaticFileDirectoryAlias = customStaticFileDirectoryAlias;
        }

        /// <summary>
        /// Only use this for tests!
        /// </summary>
        internal static JsonConfigModel Create(string? mvcHelperClassName = null, string? pageHelperClassName = null, string? linksHelperClassName = null, string? staticFilesPath = null, bool useVirtualPathProcessor = false, bool? useProcessedPathForContentLink = null, bool makeGeneratedClassesInternal = false, string? generatedClassNamespace = null, bool enableSubfoldersInViews = false, string[]? excludedStaticFileExtensions = null, string[]? excludedStaticFileDirectories = null, IReadOnlyDictionary<string, string>? additionalStaticFilesPaths = null, IReadOnlyDictionary<string, string>? customStaticFileDirectoryAlias = null)
            => new(mvcHelperClassName, pageHelperClassName, linksHelperClassName, staticFilesPath, useVirtualPathProcessor, useProcessedPathForContentLink, makeGeneratedClassesInternal, generatedClassNamespace, enableSubfoldersInViews, excludedStaticFileExtensions, excludedStaticFileDirectories, additionalStaticFilesPaths, customStaticFileDirectoryAlias);
    }
}
