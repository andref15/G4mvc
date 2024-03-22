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
public class G4ContentTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
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

    private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;

    [HtmlAttributeName(_attributeName)]
    public G4mvcContentLink Content { get; set; } = null!;

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(_attributeName);

        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        switch (output.TagName)
        {
            case _embed or _iframe or _img or _script or _source or _track or _video:
                output.Attributes.SetAttribute("src", Content.ToContentUrl(urlHelper));
                break;
            case _link:
                output.Attributes.SetAttribute("href", Content.ToContentUrl(urlHelper));
                break;
            case _object:
                output.Attributes.SetAttribute("data", Content.ToContentUrl(urlHelper));
                break;
            default:
                break;
        }

        return base.ProcessAsync(context, output);
    }
}
