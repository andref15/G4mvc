namespace G4mvc.Generator.Contexts;
internal class ParameterContext
{
    public ParameterSyntax Syntax { get; }
    public IParameterSymbol Symbol { get; }

    public ParameterContext(ParameterSyntax syntax, IParameterSymbol symbol)
    {
        Syntax = syntax;
        Symbol = symbol;
    }
}
