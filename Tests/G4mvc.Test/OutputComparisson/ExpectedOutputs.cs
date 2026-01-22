using G4mvc.Generator.Compilation;

namespace G4mvc.Test.OutputComparisson;

internal partial class ExpectedOutputs(bool disableMvcHelperSourceGeneration, bool disablePageHelperSourceGeneration, bool disableLinksHelperSourceGeneration, string? mvcClassName = null, string? pageHelperClassName = null, string? linksHelperClassName = null, bool altRoot = false, bool additionalStatic = false, bool withVpp = false, bool vppForContent = false, bool classesInternal = false, bool enumerateSubDirectories = false, string? classNamespace = null, bool excludeIco = false, bool excludeCss = false, bool customJsName = false)
{
    private readonly bool _disableMvcHelperSourceGeneration = disableMvcHelperSourceGeneration;
    private readonly bool _disablePageHelperSourceGeneration = disablePageHelperSourceGeneration;
    private readonly bool _disableLinksHelperSourceGeneration = disableLinksHelperSourceGeneration;
    private readonly string? _mvcClassName = mvcClassName ?? Configuration.JsonConfigModel.DefaultMvcHelperClassName;
    private readonly string _pageHelperClassName = pageHelperClassName ?? Configuration.JsonConfigModel.DefaultPageHelperClassName;
    private readonly string? _linksHelperClassName = linksHelperClassName ?? Configuration.JsonConfigModel.DefaultLinksHelperClassName;
    private readonly bool _altRoot = altRoot;
    private readonly bool _additionalStatic = additionalStatic;
    private readonly bool _withVpp = withVpp;
    private readonly string _vppForContentStr = vppForContent ? "true" : "false";
    private readonly bool _classesInternal = classesInternal;
    private readonly bool _enumerateSubDirectories = enumerateSubDirectories;
    private readonly string? _classNamespace = classNamespace ?? Configuration.GetRootNamespaceFromConfigValue(Configuration.JsonConfigModel.DefaultGeneratedClassNamespace, $"{nameof(G4mvc)}.{nameof(Test)}");
    private readonly bool _excludeIco = excludeIco;
    private readonly bool _excludeCss = excludeCss;
    private readonly bool _customJsName = customJsName;

    public partial Dictionary<string, string> Get();
}
