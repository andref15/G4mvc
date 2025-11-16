using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace G4mvc.TestBase.Providers;

public class G4mvcAnalyzerConfigOptionsProvider(string rootNamspace) : AnalyzerConfigOptionsProvider
{
    private readonly string _rootNamspace = rootNamspace;

    public override AnalyzerConfigOptions GlobalOptions => new G4mvcAnalyzerConfigOptions(_rootNamspace);

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        => new G4mvcAnalyzerConfigOptions(_rootNamspace);

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        => new G4mvcAnalyzerConfigOptions(_rootNamspace);

    public class G4mvcAnalyzerConfigOptions(string rootNamspace) : AnalyzerConfigOptions
    {
        private readonly string _rootNamspace = rootNamspace;
        public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
        {
            if (key is "build_property.projectdir")
            {
                value = Environment.CurrentDirectory;

                return true;
            }

            if (key is "build_property.rootnamespace")
            {
                value = _rootNamspace;

                return true;
            }

            value = null;
            return false;
        }
    }
}
