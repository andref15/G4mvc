namespace G4mvc.Test.OutputComparisson;

internal partial class ExpectedOutputs(string? mvcClassName = null, string? linksHelperClassName = null, bool altRoot = false, bool additionalStatic = false, bool withVpp = false, bool vppForContent = false, bool classesInternal = false, bool enumerateSubDirectories = false, string? classNamespace = null, bool excludeIco = false, bool excludeCss = false, bool customJsName = false)
{
    private readonly string? _mvcClassName = mvcClassName;
    private readonly string? _linksHelperClassName = linksHelperClassName;
    private readonly bool _altRoot = altRoot;
    private readonly bool _additionalStatic = additionalStatic;
    private readonly bool _withVpp = withVpp;
    private readonly bool _vppForContent = vppForContent;
    private readonly string _vppForContentStr = vppForContent ? "true" : "false";
    private readonly bool _classesInternal = classesInternal;
    private readonly bool _enumerateSubDirectories = enumerateSubDirectories;
    private readonly string? _classNamespace = classNamespace;
    private readonly bool _excludeIco = excludeIco;
    private readonly bool _excludeCss = excludeCss;
    private readonly bool _customJsName = customJsName;

    public partial Dictionary<string, string> Get();
}
