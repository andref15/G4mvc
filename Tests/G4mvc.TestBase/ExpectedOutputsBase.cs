namespace G4mvc.TestBase;
public abstract class ExpectedOutputsBase(string? mvcClassName, string? linksClassName, bool altRoot, bool additionalStatic, bool withVpp, bool classesInternal, bool excludeIco, bool excludeCss, bool customJsName)
{
    private const string _mvcDefault = "MVC";
    private const string _linksDefault = "Links";

    protected readonly bool AltRoot = altRoot;
    protected readonly bool AdditionalStatic = additionalStatic;
    protected readonly bool WithVpp = withVpp;
    protected readonly bool ClassesInternal = classesInternal;
    protected readonly bool ExcludeIco = excludeIco;
    protected readonly bool ExcludeCss = excludeCss;
    protected readonly bool CustomJsName = customJsName;
    protected readonly string LinksClassName = linksClassName ?? _linksDefault;
    protected readonly string MvcClassName = mvcClassName ?? _mvcDefault;

    public abstract string LinksClass { get; }
    public abstract string MvcClass { get; }
    public abstract string SharedClass { get; }
    public abstract string TestPartialClass { get; }
    public abstract string TestRoutesClass { get; }
    public abstract string TestPartialRoutesClass { get; }
}