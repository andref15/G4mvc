namespace G4mvc.TestBase;
public abstract class ExpectedOutputsBase(string? mvcClassName, string? linksHelperClassName, bool altRoot, bool additionalStatic, bool withVpp, bool vppForContent, bool classesInternal, string? classNamespace, bool enumerateSubDirectories, bool excludeIco, bool excludeCss, bool customJsName)
{
    private const string _mvcDefault = "MVC";
    private const string _linksDefault = "Links";

    protected readonly bool AltRoot = altRoot;
    protected readonly bool AdditionalStatic = additionalStatic;
    protected readonly bool WithVpp = withVpp;
    protected readonly bool VppForContent = vppForContent;
    protected readonly string VppForContentStr = vppForContent ? "true" : "false";
    protected readonly bool ClassesInternal = classesInternal;
    protected readonly string? ClassNamespace = classNamespace;
    protected readonly bool EnumerateSubDirectories = enumerateSubDirectories;
    protected readonly bool ExcludeIco = excludeIco;
    protected readonly bool ExcludeCss = excludeCss;
    protected readonly bool CustomJsName = customJsName;
    protected readonly string LinksHelperClassName = linksHelperClassName ?? _linksDefault;
    protected readonly string MvcClassName = mvcClassName ?? _mvcDefault;

    public abstract string LinksClass { get; }
    public abstract string MvcClass { get; }
    public abstract string SharedClass { get; }
    public abstract string TestPartialClass { get; }
    public abstract string TestRoutesClass { get; }
    public abstract string TestPartialRoutesClass { get; }
}