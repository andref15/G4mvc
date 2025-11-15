#if NETCOREAPP
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

[HtmlTargetElement(_anker, Attributes = _attributeName)]
[HtmlTargetElement(_form, Attributes = _attributeName)]
public class G4PageTagHelper(IUrlHelperFactory urlHelperFactory, IHtmlGenerator htmlGenerator) : G4RouteValuesTagHelper<G4mvcPageRouteValues>(urlHelperFactory, htmlGenerator)
{
    private const string _anker = "a";
    private const string _form = "form";
    private const string _attributeName = "g4-page";

    public override Task PostProcessFormTagAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!context.AllAttributes.TryGetAttribute("method", out var attribute) || string.IsNullOrEmpty(attribute.Value?.ToString()))
        {
            output.Attributes.SetAttribute("method", RouteValues.Method);
        }

        return Task.CompletedTask;
    }
}

#endif