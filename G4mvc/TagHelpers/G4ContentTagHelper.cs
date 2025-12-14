#if !NETSTANDARD
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

[HtmlTargetElement(_embed, Attributes = _attributeName)]
[HtmlTargetElement(_iframe, Attributes = _attributeName)]
[HtmlTargetElement(_img, Attributes = _attributeName)]
[HtmlTargetElement(_object, Attributes = _attributeName)]
[HtmlTargetElement(_script, Attributes = _attributeName)]
[HtmlTargetElement(_source, Attributes = _attributeName)]
[HtmlTargetElement(_link, Attributes = _attributeName)]
[HtmlTargetElement(_track, Attributes = _attributeName)]
[HtmlTargetElement(_video, Attributes = _attributeName)]
public class G4ContentTagHelper(IUrlHelperFactory urlHelperFactory, IFileVersionProvider fileVersionProvider) : TagHelper
{
    private const string _embed = "embed";
    private const string _iframe = "iframe";
    private const string _img = "img";
    private const string _object = "object";
    private const string _script = "script";
    private const string _source = "source";
    private const string _link = "link";
    private const string _track = "track";
    private const string _video = "video";
    private const string _attributeName = "g4-content";
    private const string _appendVersionAttributeName = "asp-append-version";

    private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;
    private readonly IFileVersionProvider _fileVersionProvider = fileVersionProvider;

    [HtmlAttributeName(_attributeName)]
    public G4mvcContentLink Content { get; set; } = null!;

    [HtmlAttributeName(_appendVersionAttributeName)]
    public bool? AppendVersion { get; set; }

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        var sourceAttributeName = output.TagName switch
        {
            _embed or _iframe or _img or _script or _source or _track or _video => "src",
            _link => "href",
            _ => null
        };

        if (sourceAttributeName is not null)
        {
            var url = Content.ToContentUrl(urlHelper);

            if (AppendVersion ?? false)
            {
                var requestPathBase = ViewContext.HttpContext.Request.PathBase;
                url = _fileVersionProvider.AddFileVersionToPath(requestPathBase, url);
            }

            output.Attributes.SetAttribute(sourceAttributeName, url);
        }

        return base.ProcessAsync(context, output);
    }
}

#endif