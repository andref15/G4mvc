namespace G4mvc.Generator;
internal class LinksGenerator
{
    public static void AddLinksClass(SourceProductionContext context, Configuration configuration, string projectDir
#if DEBUG
        , int linksVersion 
#endif
        )
    {
        var sourceBuilder = configuration.CreateSourceBuilder();

        sourceBuilder.Nullable(configuration.GlobalNullable);

        var customStaticFileDirectoryClassNames = configuration.JsonConfig.CustomStaticFileDirectoryAlias?.ToDictionary(kvp => new DirectoryInfo(Path.Combine(projectDir, kvp.Key)).FullName, kvp => kvp.Value) ?? new Dictionary<string, string>();

        var excludedDirectories = configuration.JsonConfig.ExcludedStaticFileDirectories?.Select(d => new DirectoryInfo(Path.Combine(projectDir, d)).FullName).ToList() ?? new List<string>();
        var additionalStaticFilesPaths = configuration.JsonConfig.AdditionalStaticFilesPaths;
        var linksClassName = configuration.JsonConfig.LinksClassName;
        var linksClassNameSpan = linksClassName.AsSpan();

        if (configuration.JsonConfig.UseVirtualPathProcessor)
        {
            using (sourceBuilder.BeginClass("internal static partial", "VirtualPathProcessor"))
            {
                sourceBuilder.AppendPartialMethod("public static", "string", "Process", "string path");
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
            CreateLinksClass(sourceBuilder, new(root), root, null, excludedDirectories, linksClassNameSpan, configuration, linkIdentifierParser, context.CancellationToken);

            if (additionalStaticFilesPaths is not null)
            {
                sourceBuilder.AppendLine();

                foreach (var additionalStaticFilesPath in additionalStaticFilesPaths)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    DirectoryInfo additionalRoot = new(Path.Combine(projectDir, additionalStaticFilesPath.Value));

                    var directoryClassName = linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(additionalRoot, linksClassNameSpan);
                    using (sourceBuilder.BeginClass("public static partial", $"{directoryClassName}"))
                    {
                        CreateLinksClass(sourceBuilder, additionalRoot, additionalRoot.FullName, additionalStaticFilesPath.Key.Trim('/'), excludedDirectories, linksClassNameSpan, configuration, linkIdentifierParser, context.CancellationToken);
                    }
                }
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        context.AddGeneratedSource(configuration.JsonConfig.LinksClassName, sourceBuilder);
    }

    private static void CreateLinksClass(SourceBuilder sourceBuilder, DirectoryInfo directory, string root, string? subRoute, List<string> excludedDirectories, ReadOnlySpan<char> enclosingClass, Configuration configuration, LinkIdentifierParser linkIdentifierParser, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var files = directory.GetFiles();
        var subDirectories = directory.GetDirectories();

        sourceBuilder.AppendConst("public", "string", "UrlPath", SourceCode.String(GetRelativePath(root, subRoute, directory.FullName)));

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (configuration.JsonConfig.ExcludedStaticFileExtensions != null && configuration.JsonConfig.ExcludedStaticFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            if (configuration.JsonConfig.UseVirtualPathProcessor)
            {
                sourceBuilder.AppendField("public static readonly", "string", linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(file, enclosingClass), $"VirtualPathProcessor.Process(\"{GetRelativePath(root, subRoute, file.FullName)}\")");
            }
            else
            {
                sourceBuilder.AppendConst("public", "string", linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(file, enclosingClass), SourceCode.String(GetRelativePath(root, subRoute, file.FullName)));
            }
        }

        foreach (var subDirectory in subDirectories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (excludedDirectories.Contains(subDirectory.FullName))
            {
                continue;
            }

            sourceBuilder.AppendLine();

            var newClassName = linkIdentifierParser.GetConfigAliasOrIdentifierFromPath(subDirectory, enclosingClass);
            using (sourceBuilder.BeginClass("public static partial", $"{newClassName}"))
            {
                CreateLinksClass(sourceBuilder, subDirectory, root, subRoute, excludedDirectories, newClassName.AsSpan(), configuration, linkIdentifierParser, cancellationToken);
            }
        }
    }

    private static string GetRelativePath(string root, string? subRoute, string path)
        => path.Replace(root, subRoute is null ? "~" : $"~/{subRoute}").Replace('\\', '/').TrimEnd('/');

    
}
