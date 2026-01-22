using System.Runtime.CompilerServices;

namespace G4mvc.Test.Utils;

internal static class DirectoryInfoExtensions
{
    extension(DirectoryInfo directoryInfo)
    {
        public IAsyncEnumerable<(FileInfo File, StreamReader FileContent)> EnumerateFilesWithStream(string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Internal(directoryInfo, searchPattern, searchOption);

            static async IAsyncEnumerable<(FileInfo, StreamReader)> Internal(DirectoryInfo directoryInfo, string searchPattern, SearchOption searchOption, [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                foreach (var fileInfo in directoryInfo.EnumerateFiles(searchPattern, searchOption))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using var fs = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var sr = new StreamReader(fs);

                    yield return (fileInfo, sr);
                }
            }
        }
    }
}
