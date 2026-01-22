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
            await foreach (var (file, content) in rootDirectory.EnumerateFilesWithStream("*.cshtml", SearchOption.AllDirectories).WithCancellation(cancellationToken))
            {
                sourceFileCollection.Add((file.FullName, await content.ReadToEndAsync(cancellationToken)));
            }
        }

        public async Task AddCsFilesAsync(DirectoryInfo rootDirectory, CancellationToken cancellationToken)
        {
            await foreach (var (file, content) in rootDirectory.EnumerateFilesWithStream("*.cs", SearchOption.AllDirectories))
            {
                var text = SourceText.From(await content.ReadToEndAsync(cancellationToken), Encoding.UTF8);
                sourceFileCollection.Add((file.FullName, text));
            }
        }
    }
}
