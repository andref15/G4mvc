
using Microsoft.CodeAnalysis.Testing;

namespace G4mvc.Test.OutputComparisson;

internal static class OutputComparissonExtensions
{
    extension(SourceFileCollection sourceFileCollection)
    {
        public void AddExpectedOutputs(ExpectedOutputs expectedOutputs)
        {
            foreach (var (fileName, content) in expectedOutputs.Get())
            {
                sourceFileCollection.Add((fileName, content));
            }
        }
    }
}
