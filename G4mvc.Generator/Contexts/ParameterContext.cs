namespace G4mvc.Generator.Contexts;
internal class ParameterContext(ParameterSyntax syntax, IParameterSymbol symbol)
{
    public ParameterSyntax Syntax { get; } = syntax;
    public IParameterSymbol Symbol { get; } = symbol;
}
