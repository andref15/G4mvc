using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace G4mvc.TestBase.Utils;
public static class SyntaxUtils
{
    public static SyntaxTree ToSyntaxTree(string script, CSharpParseOptions parseOptions)
    {
        var sourceText = SourceText.From(script);
        return SyntaxFactory.ParseSyntaxTree(sourceText, parseOptions);
    }

    public static bool AreEquivalent(SyntaxTree left, string right, CSharpParseOptions parseOptions)
        => left.IsEquivalentTo(ToSyntaxTree(right, parseOptions));

}
