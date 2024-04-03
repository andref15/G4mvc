﻿namespace G4mvc.Generator;
internal class LinksGenerator
{
    private const string _vppClassName = "VirtualPathProcessor";
    private const string _vppMethodName = "Process";

    private static readonly HashSet<string> _existingLinksClasses = [];

    public static void AddLinksClass(SourceProductionContext context, Configuration configuration
#if DEBUG
        , int linksVersion 
#endif
        )
    {
        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder
            .Using(nameof(G4mvc))
            .AppendLine()
            .Nullable(configuration.GlobalNullable);

        var projectDir = configuration.AnalyzerConfigValues.ProjectDir;

        var customStaticFileDirectoryClassNames = configuration.JsonConfig.CustomStaticFileDirectoryAlias?.ToDictionary(kvp => new DirectoryInfo(Path.Combine(projectDir, kvp.Key)).FullName, kvp => kvp.Value) ?? [];

        var excludedDirectories = configuration.JsonConfig.ExcludedStaticFileDirectories?.Select(d => new DirectoryInfo(Path.Combine(projectDir, d)).FullName).ToList() ?? [];
        var additionalStaticFilesPaths = configuration.JsonConfig.AdditionalStaticFilesPaths;
        var linksClassName = configuration.JsonConfig.LinksClassName;
        var linksClassNameSpan = linksClassName.AsSpan();

        if (configuration.JsonConfig.UseVirtualPathProcessor)
        {
            using (sourceBuilder.BeginClass("internal static partial", _vppClassName))
            {
                sourceBuilder.AppendPartialMethod("public static", "string", _vppMethodName, "string path");
            }

            sourceBuilder.AppendLine();
        }

        using (sourceBuilder.BeginClass($"{configuration.GeneratedClassModifier} static partial", linksClassName))
        {
#if DEBUG
            sourceBuilder.AppendLine($"//v{linksVersion}");
#endif

            LinkIdentifierParser linkIdentifierParser = new(configuration, projectDir);
            var root = Path.Combine(projectDir, configuration.JsonConfig.StaticFilesPath);
            var classPath = "";
            _existingLinksClasses.Clear();

            CreateLinksClass(sourceBuilder, new(root), root, null, excludedDirectories, linksClassNameSpan, configuration, linkIdentifierParser, classPath, context.CancellationToken);

            if (additionalStaticFilesPaths is not null)
            {
                CreateAdditionalStaticFilesLinks(context, configuration, projectDir, sourceBuilder, excludedDirectories, additionalStaticFilesPaths, linksClassNameSpan, linkIdentifierParser);
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        context.AddGeneratedSource(configuration.JsonConfig.LinksClassName, sourceBuilder);
    }

    private static void CreateAdditionalStaticFilesLinks(SourceProductionContext context, Configuration configuration, string projectDir, SourceBuilder sourceBuilder, List<string> excludedDirectories, IReadOnlyDictionary<string, string> additionalStaticFilesPaths, ReadOnlySpan<char> linksClassNameSpan, LinkIdentifierParser linkIdentifierParser)
    {
        sourceBuilder.AppendLine();

        foreach (var additionalStaticFilesPath in additionalStaticFilesPaths)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            DirectoryInfo additionalRoot = new(Path.Combine(projectDir, additionalStaticFilesPath.Key));
            var additionalVirtualPathRoot = additionalStaticFilesPath.Value.Trim('/');
            var additionalVirtualPathRootSegments = additionalVirtualPathRoot.Split('/');

            var parentSegmentClasses = new Queue<IDisposable>();
            var enclosing = linksClassNameSpan;
            var classPath = "";

            parentSegmentClasses = [];

            var urlPath = "~";

            foreach (var segment in additionalVirtualPathRootSegments)
            {
                var segmentClassName = LinkIdentifierParser.CreateIdentifierFromPath(segment, enclosing);

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

    private static void CreateLinksClass(SourceBuilder sourceBuilder, DirectoryInfo directory, string root, string? subRoute, List<string> excludedDirectories, ReadOnlySpan<char> enclosingClass, Configuration configuration, LinkIdentifierParser linkIdentifierParser, string classPath, CancellationToken cancellationToken)
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

    private static void CreateFileFields(SourceBuilder sourceBuilder, string root, string? subRoute, ReadOnlySpan<char> enclosingClass, Configuration.JsonConfigClass jsonConfig, LinkIdentifierParser linkIdentifierParser, IOrderedEnumerable<FileInfo> files, CancellationToken cancellationToken)
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

    private static void CreateSubClasses(SourceBuilder sourceBuilder, string root, string? subRoute, List<string> excludedDirectories, ReadOnlySpan<char> enclosingClass, Configuration configuration, LinkIdentifierParser linkIdentifierParser, IOrderedEnumerable<DirectoryInfo> subDirectories, string parentClassPath, CancellationToken cancellationToken)
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
