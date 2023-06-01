﻿namespace G4mvc.Generator.Helpers;
internal class LinkIdentifierParser
{
    private readonly Dictionary<string, string> _customStaticFileDirectoryClassNames;

    public LinkIdentifierParser(Configuration configuration, string projectDir)
    {
        _customStaticFileDirectoryClassNames = configuration.JsonConfig.CustomStaticFileDirectoryAlias?.ToDictionary(kvp => new DirectoryInfo(Path.Combine(projectDir, kvp.Key)).FullName, kvp => kvp.Value) ?? new Dictionary<string, string>();
    }

    public string GetConfigAliasOrIdentifierFromPath(FileSystemInfo fileSystemInfo, ReadOnlySpan<char> enclosing)
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
