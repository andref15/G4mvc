using G4mvc.Generator.Compilation;

namespace G4mvc.Test.OutputComparisson;

internal class ConfigAndExpectedOutputBuilder
{
    private const string _mvcDefault = "MVC";
    private const string _razorDefault = "RazorPages";
    private const string _linksDefault = "Links";

    private bool _disableMvcHelperSourceGeneration;
    private bool _disablePageHelperSourceGeneration;
    private bool _disableLinksHelperSourceGeneration;
    private string _mvcClassName = _mvcDefault;
    private string _pageHelperClassName = _razorDefault;
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

    private ConfigAndExpectedOutputBuilder()
    {

    }

    public static ConfigAndExpectedOutputBuilder Create()
    {
        return new();
    }

    public ConfigAndExpectedOutputBuilder DisableMvcHelperSourceGeneration(bool disable = true)
    {
        _disableMvcHelperSourceGeneration = disable;
        return this;
    }

    public ConfigAndExpectedOutputBuilder DisablePageHelperSourceGeneration(bool disable = true)
    {
        _disablePageHelperSourceGeneration = disable;
        return this;
    }

    public ConfigAndExpectedOutputBuilder DisableLinksHelperSourceGeneration(bool disable = true)
    {
        _disableLinksHelperSourceGeneration = disable;
        return this;
    }

    public ConfigAndExpectedOutputBuilder WithMvcClassName(string name)
    {
        _mvcClassName = name;
        return this;
    }

    public ConfigAndExpectedOutputBuilder WithPageHelperClassName(string name)
    {
        _pageHelperClassName = name;
        return this;
    }

    public ConfigAndExpectedOutputBuilder WithLinksClassName(string name)
    {
        _linksHelperClassName = name;
        return this;
    }

    public ConfigAndExpectedOutputBuilder UseAlternativeRoot(bool use = true)
    {
        _altRoot = use;
        return this;
    }

    public ConfigAndExpectedOutputBuilder UseAdditionalStaticFilesPath(bool use = true)
    {
        _additionalStatic = use;
        return this;
    }

    public ConfigAndExpectedOutputBuilder UseVirtualPathProcessor(bool use = true)
    {
        _withVpp = use;
        return this;
    }

    public ConfigAndExpectedOutputBuilder UseVirtualPathProcessorForContentLinks(bool use = true)
    {
        _vppForContent = use;
        return this;
    }

    public ConfigAndExpectedOutputBuilder MakeGeneratedClassesInternal(bool makeInternal = true)
    {
        _classesInternal = makeInternal;
        return this;
    }

    public ConfigAndExpectedOutputBuilder WithClassNamespace(string @namespace = "global")
    {
        _classNamespace = @namespace;
        return this;
    }

    public ConfigAndExpectedOutputBuilder EnumerateSubDirectories(bool enumerate = true)
    {
        _enumerateSubDirectories = enumerate;
        return this;
    }

    public ConfigAndExpectedOutputBuilder ExcludeIco(bool exclude = true)
    {
        _excludeIco = exclude;
        return this;
    }

    public ConfigAndExpectedOutputBuilder ExcludeCss(bool exclude = true)
    {
        _excludeCss = exclude;
        return this;
    }

    public ConfigAndExpectedOutputBuilder UseCustomJsName(bool use = true)
    {
        _customJsName = use;
        return this;
    }

    public (Configuration.JsonConfigModel JsonConfig, ExpectedOutputs ExpectedOutputs) Build()
    {
        var jsonConfig = Configuration.JsonConfigModel.Create(_disableMvcHelperSourceGeneration, _disablePageHelperSourceGeneration, _disableLinksHelperSourceGeneration, _mvcClassName, _pageHelperClassName, _linksHelperClassName, _altRoot ? "wwwrootAlt" : "wwwroot", _withVpp, _vppForContent, _classesInternal, _classNamespace, _enumerateSubDirectories,
            _excludeIco ? [".ico"] : [],
            _excludeCss ? ["wwwroot/css"] : [],
            _additionalStatic ? new Dictionary<string, string>() { ["wwwrootAlt"] = "alt" } : [],
            _customJsName ? new Dictionary<string, string>() { ["wwwroot/js"] = "otherjs" } : []
        );
        var expectedOutputs = new ExpectedOutputs(_disableMvcHelperSourceGeneration, _disablePageHelperSourceGeneration, _disableLinksHelperSourceGeneration, _mvcClassName, _pageHelperClassName, _linksHelperClassName, _altRoot, _additionalStatic, _withVpp, _vppForContent, _classesInternal, _enumerateSubDirectories, _classNamespace, _excludeIco, _excludeCss, _customJsName);

        return (jsonConfig, expectedOutputs);
    }
}
