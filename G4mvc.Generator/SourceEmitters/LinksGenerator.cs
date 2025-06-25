using G4mvc.Generator.Compilation;

namespace G4mvc.Generator.SourceEmitters;
internal class LinksGenerator
{
    private const string _vppClassName = "VirtualPathProcessor";
    private const string _vppMethodName = "Process";

#if DEBUG
    private int _version = 0;
#endif

    private readonly HashSet<string> _existingLinksClasses = [];

    public void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<Configuration> configuration)
    {
        context.RegisterSourceOutput(configuration, ExecuteLinksGeneration);
    }

    private void ExecuteLinksGeneration(SourceProductionContext context, Configuration configuration)
    {
#if DEBUG
        _version++;
#endif
        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder
            .Using(nameof(G4mvc))
            .AppendLine();

        IDisposable? namespaceDisposable = null;

        if (configuration.GeneratedClassNamespace is not null)
        {
            namespaceDisposable = sourceBuilder.BeginNamespace(configuration.GeneratedClassNamespace, true);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Nullable(configuration.GlobalNullable);

        var projectDir = configuration.AnalyzerConfigValues.ProjectDir;

        var customStaticFileDirectoryClassNames = configuration.JsonConfig.CustomStaticFileDirectoryAlias?.ToDictionary(kvp => new DirectoryInfo(Path.Combine(projectDir, kvp.Key)).FullName, kvp => kvp.Value) ?? [];

        var excludedDirectories = configuration.JsonConfig.ExcludedStaticFileDirectories?.Select(d => new DirectoryInfo(Path.Combine(projectDir, d)).FullName).ToList() ?? [];
        var additionalStaticFilesPaths = configuration.JsonConfig.AdditionalStaticFilesPaths;
        var linksHelperClassName = configuration.JsonConfig.LinksHelperClassName;
        var linksHelperClassNameSpan = linksHelperClassName.AsSpan();

        if (configuration.JsonConfig.UseVirtualPathProcessor)
        {
            using (sourceBuilder.BeginClass("internal static partial", _vppClassName))
            {
                sourceBuilder.AppendPartialMethod("public static", "string", _vppMethodName, "string path");
            }

            sourceBuilder.AppendLine();
        }

        using (sourceBuilder.BeginClass($"{configuration.GeneratedClassModifier} static partial", linksHelperClassName))
        {
#if DEBUG
            sourceBuilder.AppendLine($"//v{_version}");
#endif

            var linkIdentifierParser = new IdentifierParser(configuration, projectDir);
            var root = Path.Combine(projectDir, configuration.JsonConfig.StaticFilesPath);
            var classPath = "";
            _existingLinksClasses.Clear();

            CreateLinksClass(sourceBuilder, new(root), root, null, excludedDirectories, linksHelperClassNameSpan, configuration, linkIdentifierParser, classPath, context.CancellationToken);

            if (additionalStaticFilesPaths is not null)
            {
                CreateAdditionalStaticFilesLinks(context, configuration, projectDir, sourceBuilder, excludedDirectories, additionalStaticFilesPaths, linksHelperClassNameSpan, linkIdentifierParser);
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        namespaceDisposable?.Dispose();

        context.AddGeneratedSource(configuration.JsonConfig.LinksHelperClassName, sourceBuilder);
    }

    private void CreateAdditionalStaticFilesLinks(SourceProductionContext context, Configuration configuration, string projectDir, SourceBuilder sourceBuilder, List<string> excludedDirectories, IReadOnlyDictionary<string, string> additionalStaticFilesPaths, ReadOnlySpan<char> linksHelperClassNameSpan, IdentifierParser linkIdentifierParser)
    {
        sourceBuilder.AppendLine();

        foreach (var additionalStaticFilesPath in additionalStaticFilesPaths)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var additionalRoot = new DirectoryInfo(Path.Combine(projectDir, additionalStaticFilesPath.Key));
            var additionalVirtualPathRoot = additionalStaticFilesPath.Value.Trim('/');
            var additionalVirtualPathRootSegments = additionalVirtualPathRoot.Split('/');

            var parentSegmentClasses = new Queue<IDisposable>();
            var enclosing = linksHelperClassNameSpan;
            var classPath = "";

            parentSegmentClasses = [];

            var urlPath = "~";

            foreach (var segment in additionalVirtualPathRootSegments)
            {
                var segmentClassName = IdentifierParser.CreateIdentifierFromPath(segment, enclosing);

                classPath = $"{classPath}-{segmentClassName}";

                var @class = sourceBuilder.BeginClass("public static partial", segmentClassName);
                urlPath += "/" + segment;

                if (!_existingLinksClasses.Contains(classPath))
                {
                    sourceBuilder.AppendConst("public", "string", "UrlPath", SourceCode.String(urlPath));
                }

                _existingLinksClasses.Add(classPath);

                parentSegmentClasses.Enqueue(@class);
                enclosing = segmentClassName.AsSpan();
            }

            CreateFileFields(sourceBuilder, additionalRoot.FullName, additionalVirtualPathRoot, enclosing, configuration.JsonConfig, linkIdentifierParser, additionalRoot.EnumerateFiles().OrderBy(f => f.Name), context.CancellationToken);
            CreateSubClasses(sourceBuilder, additionalRoot.FullName, additionalVirtualPathRoot, excludedDirectories, enclosing, configuration, linkIdentifierParser, additionalRoot.EnumerateDirectories().OrderBy(d => d.Name), classPath, context.CancellationToken);

            while (parentSegmentClasses.Count > 0)
            {
                parentSegmentClasses.Dequeue().Dispose();
            }
        }
    }

    private void CreateLinksClass(SourceBuilder sourceBuilder, DirectoryInfo directory, string root, string? subRoute, List<string> excludedDirectories, ReadOnlySpan<char> enclosingClass, Configuration configuration, IdentifierParser linkIdentifierParser, string classPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var files = directory.EnumerateFiles().OrderBy(f => f.Name);
        var subDirectories = directory.EnumerateDirectories().OrderBy(d => d.Name);

        if (!_existingLinksClasses.Contains(classPath))
        {
            sourceBuilder.AppendConst("public", "string", "UrlPath", SourceCode.String(GetRelativePath(root, subRoute, directory.FullName)));
        }

        CreateFileFields(sourceBuilder, root, subRoute, enclosingClass, configuration.JsonConfig, linkIdentifierParser, files, cancellationToken);
        CreateSubClasses(sourceBuilder, root, subRoute, excludedDirectories, enclosingClass, configuration, linkIdentifierParser, subDirectories, classPath, cancellationToken);
    }

    private static void CreateFileFields(SourceBuilder sourceBuilder, string root, string? subRoute, ReadOnlySpan<char> enclosingClass, Configuration.JsonConfigModel jsonConfig, IdentifierParser linkIdentifierParser, IOrderedEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (jsonConfig.ExcludedStaticFileExtensions != null && jsonConfig.ExcludedStaticFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            if (jsonConfig.UseVirtualPathProcessor)
            {
                sourceBuilder.AppendField("public static readonly", nameof(G4mvcContentLink), linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(file, enclosingClass), $"new(\"{GetRelativePath(root, subRoute, file.FullName)}\", {_vppClassName}.{_vppMethodName}, {(jsonConfig.UseProcessedPathForContentLink ? "true" : "false")})");
            }
            else
            {
                sourceBuilder.AppendField("public static readonly", nameof(G4mvcContentLink), linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(file, enclosingClass), $"new(\"{GetRelativePath(root, subRoute, file.FullName)}\")");
            }
        }
    }

    private void CreateSubClasses(SourceBuilder sourceBuilder, string root, string? subRoute, List<string> excludedDirectories, ReadOnlySpan<char> enclosingClass, Configuration configuration, IdentifierParser linkIdentifierParser, IOrderedEnumerable<DirectoryInfo> subDirectories, string parentClassPath, CancellationToken cancellationToken)
    {

        foreach (var subDirectory in subDirectories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (excludedDirectories.Contains(subDirectory.FullName))
            {
                continue;
            }

            sourceBuilder.AppendLine();

            var newClassName = linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(subDirectory, enclosingClass);
            var subClassPath = $"{parentClassPath}-{newClassName}";

            using (sourceBuilder.BeginClass("public static partial", $"{newClassName}"))
            {
                CreateLinksClass(sourceBuilder, subDirectory, root, subRoute, excludedDirectories, newClassName.AsSpan(), configuration, linkIdentifierParser, subClassPath, cancellationToken);
            }

            _existingLinksClasses.Add(subClassPath);
        }
    }

    private static string GetRelativePath(string root, string? subRoute, string path)
        => path.Replace(root, subRoute is null ? "~" : $"~/{subRoute}").Replace('\\', '/').TrimEnd('/');
}
