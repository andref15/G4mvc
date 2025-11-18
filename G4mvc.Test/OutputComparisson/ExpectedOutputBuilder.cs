namespace G4mvc.Test.OutputComparisson;

internal class ExpectedOutputBuilder
{
    private const string _mvcDefault = "MVC";
    private const string _razorDefault = "RazorPages";
    private const string _linksDefault = "Links";

    private string _mvcClassName = _mvcDefault;
    private string _razorPagesClassName = _razorDefault;
    private string _linksHelperClassName = _linksDefault;
    private bool _altRoot;
    private bool _additionalStatic;
    private bool _withVpp;
    private bool _vppForContent;
    private bool _classesInternal;
    private string? _classNamespace;
    private bool _enumerateSubDirectories;
    private bool _excludeIco;
    private bool _excludeCss;
    private bool _customJsName;

    private ExpectedOutputBuilder()
    {

    }

    public static ExpectedOutputBuilder Create()
    {
        return new();
    }

    public ExpectedOutputBuilder WithMvcClassName(string name)
    {
        _mvcClassName = name;
        return this;
    }

    public ExpectedOutputBuilder WithRazorPagesClassName(string name)
    {
        _razorPagesClassName = name;
        return this;
    }

    public ExpectedOutputBuilder WithLinksClassName(string name)
    {
        _linksHelperClassName = name;
        return this;
    }

    public ExpectedOutputBuilder UseAlternativeRoot(bool use = true)
    {
        _altRoot = use;
        return this;
    }

    public ExpectedOutputBuilder UseAdditionalStaticFilesPath(bool use = true)
    {
        _additionalStatic = use;
        return this;
    }

    public ExpectedOutputBuilder UseVirtualPathProcessor(bool use = true)
    {
        _withVpp = use;
        return this;
    }

    public ExpectedOutputBuilder UseVirtualPathProcessorForContentLinks(bool use = true)
    {
        _vppForContent = use;
        return this;
    }

    public ExpectedOutputBuilder MakeGeneratedClassesInternal(bool makeInternal = true)
    {
        _classesInternal = makeInternal;
        return this;
    }

    public ExpectedOutputBuilder WithClassNamespace(string @namespace = "global")
    {
        _classNamespace = @namespace;
        return this;
    }

    public ExpectedOutputBuilder EnumerateSubDirectories(bool enumerate = true)
    {
        _enumerateSubDirectories = enumerate;
        return this;
    }

    public ExpectedOutputBuilder ExcludeIco(bool exclude = true)
    {
        _excludeIco = exclude;
        return this;
    }

    public ExpectedOutputBuilder ExcludeCss(bool exclude = true)
    {
        _excludeCss = exclude;
        return this;
    }

    public ExpectedOutputBuilder UseCustomJsName(bool use = true)
    {
        _customJsName = use;
        return this;
    }

    public ExpectedOutputs Build()
    {
        return new ExpectedOutputs(_mvcClassName, _linksHelperClassName, _altRoot, _additionalStatic, _withVpp, _vppForContent, _classesInternal, _enumerateSubDirectories, _classNamespace, _excludeIco, _excludeCss, _customJsName);
    }
}
