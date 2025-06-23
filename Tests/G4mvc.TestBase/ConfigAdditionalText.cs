using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text.Json;

namespace G4mvc.TestBase;
internal class ConfigAdditionalText(Configuration.JsonConfigModel jsonConfig) : AdditionalText
{
    private readonly Configuration.JsonConfigModel _jsonConfig = jsonConfig;

    public override string Path => Configuration.FileName;

    public override SourceText? GetText(CancellationToken cancellationToken = default)
    {
        var text = JsonSerializer.Serialize(_jsonConfig);
        return SourceText.From(text);
    }
}
