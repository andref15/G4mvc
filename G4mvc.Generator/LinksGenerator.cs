namespace G4mvc.Generator;
internal static class @LinksGenerator
{
    public static void AddLinksClass(GeneratorExecutionContext context, string projectDir)
    {
        SourceBuilder sourceBuilder = Configuration.Instance.CreateSourceBuilder();

        sourceBuilder.Nullable(Configuration.Instance.GlobalNullable);

        string root = Path.Combine(projectDir, Configuration.Instance.JsonConfig.StaticFilesPath);

        List<string> excludedDirectories = Configuration.Instance.JsonConfig.ExcludedStaticFileDirectories?.Select(d => new DirectoryInfo(Path.Combine(root, d)).FullName).ToList() ?? new List<string>();

        using (sourceBuilder.BeginClass("public static", Configuration.Instance.JsonConfig.LinksClassName))
        {
            CreateLinksClass(sourceBuilder, new(root), root, excludedDirectories, context.CancellationToken);
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        context.AddGeneratedSource(Configuration.Instance.JsonConfig.LinksClassName, sourceBuilder);
    }

    private static void CreateLinksClass(SourceBuilder sourceBuilder, DirectoryInfo directory, string root, List<string> excludedDirectories, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        FileInfo[] files = directory.GetFiles();
        DirectoryInfo[] subDirectories = directory.GetDirectories();

        sourceBuilder.AppendConst("public", "string", "UrlPath", SourceCode.String(GetRelativePath(root, directory.FullName)));

        foreach (FileInfo file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Configuration.Instance.JsonConfig.ExcludedStaticFileExtensions != null && Configuration.Instance.JsonConfig.ExcludedStaticFileExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            sourceBuilder.AppendConst("public", "string", CreateIdentifierFromFile(file.Name), SourceCode.String(GetRelativePath(root, file.FullName)));
        }

        foreach (DirectoryInfo subDirectory in subDirectories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (excludedDirectories.Contains(subDirectory.FullName))
            {
                continue;
            }
            
            sourceBuilder.AppendLine();

            using (sourceBuilder.BeginClass("public static", $"{CreateIdentifierFromFile(subDirectory.Name)}Links"))
            {
                CreateLinksClass(sourceBuilder, subDirectory, root, excludedDirectories, cancellationToken);
            }
        }
    }

    private static string GetRelativePath(string root, string path)
        => path.Replace(root, "~").Replace('\\', '/').TrimEnd('/');

    private static string CreateIdentifierFromFile(string fileName)
    {
        ReadOnlySpan<char> span = fileName.AsSpan();

        Span<char> identifierName = stackalloc char[fileName.Length * 2];

        int idx = 0;
        identifierName[idx++] = '@';
        
        if (!SyntaxFacts.IsIdentifierStartCharacter(span[0]))
        {
            identifierName[idx++] = '_';
        }

        for (int i = 0; i < span.Length; i++, idx++)
        {
            if (SyntaxFacts.IsIdentifierPartCharacter(span[i]))
            {
                identifierName[idx] = span[i];
                continue;
            }

            if (span[i] is '-')
            {
                identifierName[idx] = char.ToUpper(span[++i]);
                continue;
            }

            identifierName[idx] = '_';
        }

        return identifierName.ToString().TrimEnd('\0');
    }
}
