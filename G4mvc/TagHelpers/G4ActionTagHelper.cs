using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;
[HtmlTargetElement(_anker, Attributes = _attributeName)]
[HtmlTargetElement(_form, Attributes = _attributeName)]
public class G4ActionTagHelper : TagHelper
{
    private const string _anker = "a";
    private const string _form = "form";
    private const string _attributeName = "g4-action";

    private readonly IUrlHelperFactory _urlHelperFactory;

    [HtmlAttributeName(_attributeName)]
    public G4mvcRouteValues Action { get; set; } = null!;

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public G4ActionTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(_attributeName);

        IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        switch (output.TagName)
        {
            case _anker:
                output.Attributes.SetAttribute("href", urlHelper.RouteUrl(Action));
                break;
            case _form:
                output.Attributes.SetAttribute("action", urlHelper.RouteUrl(Action));

                if (!context.AllAttributes.TryGetAttribute("method", out TagHelperAttribute attribute) || string.IsNullOrEmpty(attribute.Value?.ToString()))
                {
                    output.Attributes.SetAttribute("method", "POST");
                }

                break;
            default:
                break;
        }

        return base.ProcessAsync(context, output);
    }
}
