using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace G4mvc.TestBase.Providers;
public class G4mvcAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    public override AnalyzerConfigOptions GlobalOptions => new G4mvcAnalyzerConfigOptions();

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        => new G4mvcAnalyzerConfigOptions();

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        => new G4mvcAnalyzerConfigOptions();

    public class G4mvcAnalyzerConfigOptions : AnalyzerConfigOptions
    {
        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            if (key is "build_property.projectdir")
            {
                value = Environment.CurrentDirectory;

                return true;
            }

            value = null;
            return false;
        }
    }
}
