using G4mvc.TestBase.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace G4mvc.TestBase;
internal class SyntaxAssert
{
    public static void AreEquivalent(string expected, SyntaxTree actual, CSharpParseOptions parseOptions)
        => Assert.IsTrue(SyntaxUtils.AreEquivalent(actual, expected, parseOptions), "Actual SyntaxTree does not match expected syntax");

    public static void AreAnyEquivalent(string expected, IEnumerable<SyntaxTree> actual, CSharpParseOptions parseOptions)
        => Assert.IsTrue(actual.Any(st => SyntaxUtils.AreEquivalent(st, expected, parseOptions)), "No SyntaxTrees match the expected syntax");
}
