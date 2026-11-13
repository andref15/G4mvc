namespace G4mvc.Test.Utils;

internal static class DirectoryInfoExtensions
{
    extension(DirectoryInfo directoryInfo)
    {
        public IEnumerable<FileStreamReader> EnumerateFileStreamReaders(string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var file in directoryInfo.EnumerateFiles(searchPattern, searchOption))
            {
                yield return new FileStreamReader(file);
            }
        }
    }
}
