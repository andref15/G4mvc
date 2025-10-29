#if NETCOREAPP
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;
[HtmlTargetElement(_anker, Attributes = _attributeName)]
[HtmlTargetElement(_form, Attributes = _attributeName)]
public class G4PageTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
{
    private const string _anker = "a";
    private const string _form = "form";
    private const string _attributeName = "g4-page";

    private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;

    [HtmlAttributeName(_attributeName)]
    public G4mvcPageRouteValues Page { get; set; } = null!;

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(_attributeName);

        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        switch (output.TagName)
        {
            case _anker:
                output.Attributes.SetAttribute("href", urlHelper.RouteUrl(Page));
                break;
            case _form:
                output.Attributes.SetAttribute("action", urlHelper.RouteUrl(Page));

                if (!context.AllAttributes.TryGetAttribute("method", out var attribute) || string.IsNullOrEmpty(attribute.Value?.ToString()))
                {
                    output.Attributes.SetAttribute("method", Page.Method);
                }

                break;
            default:
                break;
        }

        return base.ProcessAsync(context, output);
    }
}

#endif