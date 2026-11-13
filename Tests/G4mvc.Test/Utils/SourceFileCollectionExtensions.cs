using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace G4mvc.Test.Utils;

internal static class SourceFileCollectionExtensions
{
    extension(SourceFileCollection sourceFileCollection)
    {
        public void AddEntrypoint(DirectoryInfo rootDirectory)
        {
            var text = SourceText.From("""
                using Microsoft.AspNetCore.Builder;

                var builder = WebApplication.CreateBuilder(args);
                var app = builder.Build();
                app.Run();
                """, Encoding.UTF8);

            sourceFileCollection.Add((Path.Combine(rootDirectory.FullName, "Program.cs"), text));
        }

        public async Task AddCshtmlFilesAsync(DirectoryInfo rootDirectory, CancellationToken cancellationToken)
        {
            var tasks = rootDirectory.EnumerateFileStreamReaders("*.cshtml", SearchOption.AllDirectories).Select(ReadFileNameAndContent(cancellationToken));
            await foreach (var task in Task.WhenEach(tasks))
            {
                var (fileInfo, content) = await task;
                sourceFileCollection.Add((fileInfo.FullName, content));
            }
        }

        public async Task AddCsFilesAsync(DirectoryInfo rootDirectory, CancellationToken cancellationToken)
        {
            var tasks = rootDirectory.EnumerateFileStreamReaders("*.cs", SearchOption.AllDirectories).Select(ReadFileNameAndContent(cancellationToken));
            await foreach (var task in Task.WhenEach(tasks))
            {
                var (fileInfo, content) = await task;
                var text = SourceText.From(content, Encoding.UTF8);
                sourceFileCollection.Add((fileInfo.FullName, text));
            }
        }

        private static Func<FileStreamReader, Task<(FileInfo File, string Content)>> ReadFileNameAndContent(CancellationToken cancellationToken)
        {
            return async reader =>
            {
                try
                {
                    var content = await reader.ReadToEndAsync(cancellationToken);
                    return (reader.FileInfo, content);
                }
                finally
                {
                    reader?.Dispose();
                }
            };
        }
    }
}
