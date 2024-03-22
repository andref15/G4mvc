using Microsoft.AspNetCore.Mvc;

namespace G4mvc;
public class G4mvcContentLink(string path, Func<string, string>? processor = null, bool useProcessedPathForContentUrl = false)
{
    private readonly string _path = path;
    private readonly string _processedPath = processor is null ? path : processor(path);
    private readonly bool _useProcessedPathForContentUrl = useProcessedPathForContentUrl;

    /// <summary>
    /// If VirtualPathProcessor is enabled returns the processed path, otherwise the raw path is returned
    /// </summary>
    /// <returns>The virtual path.</returns>
    public override string ToString()
        => ToString(true);

    /// <summary>
    /// Returns the processed path if <paramref name="processed"/> is set to true, otherwise the raw path is returned.
    /// </summary>
    /// <param name="processed">Set this to false if you need the unprocessed path</param>
    /// <returns>The virtual path.</returns>
    public string ToString(bool processed)
        => processed ? _processedPath : _path;

    public string ToContentUrl(IUrlHelper urlHelper)
        => urlHelper.Content(_useProcessedPathForContentUrl ? _processedPath : _path);
}
