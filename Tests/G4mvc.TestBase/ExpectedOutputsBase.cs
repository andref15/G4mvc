namespace G4mvc.TestBase;
public abstract class ExpectedOutputsBase(string? mvcClassName = null, string? linksClassName = null, bool altRoot = false, bool additionalStatic = false, bool withVpp = false, bool excludeIco = false)
{
    private const string _mvcDefault = "MVC";
    private const string _linksDefault = "Links";

    protected readonly bool AltRoot = altRoot;
    protected readonly bool AdditionalStatic = additionalStatic;
    protected readonly bool WithVpp = withVpp;
    protected readonly bool ExcludeIco = excludeIco;
    protected readonly string LinksClassName = linksClassName ?? _linksDefault;
    protected readonly string MvcClassName = mvcClassName ?? _mvcDefault;

    public abstract string LinksClass { get; }
    public abstract string MvcClass { get; }
    public abstract string SharedClass { get; }
    public abstract string TestPartialClass { get; }
    public abstract string TestRoutesClass { get; }
    public abstract string TestPartialRoutesClass { get; }
}