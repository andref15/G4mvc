#if NETCOREAPP
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

[HtmlTargetElement(_anchor, Attributes = _attributeName)]
[HtmlTargetElement(_form, Attributes = _attributeName)]
public class G4ActionTagHelper(IUrlHelperFactory urlHelperFactory, IHtmlGenerator htmlGenerator) : G4RouteValuesTagHelper<G4mvcActionRouteValues>(urlHelperFactory, htmlGenerator)
{
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