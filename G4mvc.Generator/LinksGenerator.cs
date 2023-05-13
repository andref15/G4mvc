namespace G4mvc.Generator;
internal static class LinksGenerator
{
    private static Dictionary<string, string> _customStaticFileDirectoryClassNames = null!;

    public static void AddLinksClass(SourceProductionContext context, string projectDir
#if DEBUG
        , int linksVersion 
#endif
        )
    {
        SourceBuilder sourceBuilder = Configuration.Instance.CreateSourceBuilder();

        sourceBuilder.Nullable(Configuration.Instance.GlobalNullable);

        _customStaticFileDirectoryClassNames = Configuration.Instance.JsonConfig.CustomStaticFileDirectoryAlias?.ToDictionary(kvp => new DirectoryInfo(Path.Combine(projectDir, kvp.Key)).FullName, kvp => kvp.Value) ?? new Dictionary<string, string>();

        List<string> excludedDirectories = Configuration.Instance.JsonConfig.ExcludedStaticFileDirectories?.Select(d => new DirectoryInfo(Path.Combine(projectDir, d)).FullName).ToList() ?? new List<string>();
        Dictionary<string, string>? additionalStaticFilesPaths = Configuration.Instance.JsonConfig.AdditionalStaticFilesPaths;
        string linksClassName = Configuration.Instance.JsonConfig.LinksClassName;
        ReadOnlySpan<char> linksClassNameSpan = linksClassName.AsSpan();

        using (sourceBuilder.BeginClass("public static", linksClassName))
        {
#if DEBUG
            sourceBuilder.AppendLine($"//v{linksVersion}"); 
#endif
            string root = Path.Combine(projectDir, Configuration.Instance.JsonConfig.StaticFilesPath);
            CreateLinksClass(sourceBuilder, new(root), root, null, excludedDirectories, linksClassNameSpan, context.CancellationToken);

            if (additionalStaticFilesPaths is not null)
            {
                sourceBuilder.AppendLine();

                foreach (KeyValuePair<string, string> additionalStaticFilesPath in additionalStaticFilesPaths)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    DirectoryInfo additionalRoot = new(Path.Combine(projectDir, additionalStaticFilesPath.Value));

                    string directoryClassName = GetConfigAliasOrIdentifierFromPath(additionalRoot, linksClassNameSpan);
                    using (sourceBuilder.BeginClass("public static", $"{directoryClassName}Links"))
                    {
                        CreateLinksClass(sourceBuilder, additionalRoot, additionalRoot.FullName, additionalStaticFilesPath.Key.Trim('/'), excludedDirectories, linksClassNameSpan, context.CancellationToken);
                    }
                }
            }
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        context.AddGeneratedSource(Configuration.Instance.JsonConfig.LinksClassName, sourceBuilder);
    }

    private static void CreateLinksClass(SourceBuilder sourceBuilder, DirectoryInfo directory, string root, string? subRoute, List<string> excludedDirectories, ReadOnlySpan<char> enclosingClass, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        FileInfo[] files = directory.GetFiles();
        DirectoryInfo[] subDirectories = directory.GetDirectories();

        sourceBuilder.AppendConst("public", "string", "UrlPath", SourceCode.String(GetRelativePath(root, subRoute, directory.FullName)));

        foreach (FileInfo file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Configuration.Instance.JsonConfig.ExcludedStaticFileExtensions != null && Configuration.Instance.JsonConfig.ExcludedStaticFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            sourceBuilder.AppendConst("public", "string", GetConfigAliasOrIdentifierFromPath(file, enclosingClass), SourceCode.String(GetRelativePath(root, subRoute, file.FullName)));
        }

        foreach (DirectoryInfo subDirectory in subDirectories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (excludedDirectories.Contains(subDirectory.FullName))
            {
                continue;
            }

            sourceBuilder.AppendLine();

            string newClassName = GetConfigAliasOrIdentifierFromPath(subDirectory, enclosingClass);
            using (sourceBuilder.BeginClass("public static", $"{newClassName}Links"))
            {
                CreateLinksClass(sourceBuilder, subDirectory, root, subRoute, excludedDirectories, newClassName.AsSpan(), cancellationToken);
            }
        }
    }

    private static string GetRelativePath(string root, string? subRoute, string path)
        => path.Replace(root, subRoute is null ? "~" : $"~/{subRoute}").Replace('\\', '/').TrimEnd('/');

    private static string GetConfigAliasOrIdentifierFromPath(FileSystemInfo fileSystemInfo, ReadOnlySpan<char> enclosing)
        => _customStaticFileDirectoryClassNames.TryGetValue(fileSystemInfo.FullName, out string? alias)
            ? alias
            : CreateIdentifierFromPath(fileSystemInfo.Name, enclosing);

    private static string CreateIdentifierFromPath(string pathSegment, ReadOnlySpan<char> enclosing)
    {
        ReadOnlySpan<char> span = pathSegment.AsSpan();

        Span<char> identifierName = stackalloc char[pathSegment.Length + 3];

        int idx = 0;
        identifierName[idx++] = '@';

        if (!SyntaxFacts.IsIdentifierStartCharacter(span[0]))
        {
            identifierName[idx++] = '_';
        }

        for (int i = 0; i < span.Length; i++)
        {
            if (SyntaxFacts.IsIdentifierPartCharacter(span[i]))
            {
                identifierName[idx++] = span[i];
                continue;
            }

            identifierName[idx++] = '_';
        }

        if (enclosing.Equals(identifierName.Slice(0, idx), StringComparison.Ordinal))
        {
            identifierName[idx++] = '_';
        }

        return identifierName.Slice(0, idx).ToString();
    }
}
