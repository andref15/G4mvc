
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace G4mvc.Test.OutputComparisson;

internal static class OutputComparissonExtensions
{
    extension(SourceFileCollection sourceFileCollection)
    {
        public void AddExpectedOutputs(ExpectedOutputs expectedOutputs)
        {
            foreach (var (fileName, content) in expectedOutputs.Get())
            {
                var sourceText = SourceText.From(content, Encoding.UTF8);
                sourceFileCollection.Add((fileName, sourceText));
            }
        }
    }
}
