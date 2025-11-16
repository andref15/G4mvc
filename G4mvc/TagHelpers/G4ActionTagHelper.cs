#if NETCOREAPP
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

[HtmlTargetElement(Anchor, Attributes = _attributeName)]
[HtmlTargetElement(Form, Attributes = _attributeName)]
public class G4ActionTagHelper(IUrlHelperFactory urlHelperFactory, IHtmlGenerator htmlGenerator) : G4RouteValuesTagHelper<G4mvcActionRouteValues>(_attributeName, urlHelperFactory, htmlGenerator)
{
    private const string _attributeName = "g4-action";

    [HtmlAttributeName(_attributeName)]
    public override G4mvcActionRouteValues RouteValues { get; set; } = null!;

    public override Task PostProcessFormTagAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!context.AllAttributes.TryGetAttribute("method", out var attribute) || string.IsNullOrEmpty(attribute.Value?.ToString()))
        {
            output.Attributes.SetAttribute("method", "POST");
        }

        return Task.CompletedTask;
    }
}

#endif