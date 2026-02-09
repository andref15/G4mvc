#if !NETSTANDARD
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

[HtmlTargetElement(Anchor, Attributes = _attributeName)]
[HtmlTargetElement(Form, Attributes = _attributeName)]
public class G4PageTagHelper(IUrlHelperFactory urlHelperFactory, IHtmlGenerator htmlGenerator) : G4RouteValuesTagHelper<G4mvcPageRouteValues>(_attributeName, urlHelperFactory, htmlGenerator)
{
    private const string _attributeName = "g4-page";

    [HtmlAttributeName(_attributeName)]
    public override G4mvcPageRouteValues RouteValues { get; set; } = null!;
    public override Task PostProcessFormTagAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!output.Attributes.TryGetAttribute("method", out var attribute) || string.IsNullOrEmpty(attribute.Value?.ToString()))
        {
            output.Attributes.SetAttribute("method", RouteValues.Method);
        }

        return Task.CompletedTask;
    }
}

#endif