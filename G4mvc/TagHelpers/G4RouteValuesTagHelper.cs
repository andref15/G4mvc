#if NETCOREAPP
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

[HtmlTargetElement(_anchor, Attributes = _attributeName)]
[HtmlTargetElement(_form, Attributes = _attributeName)]
public abstract class G4RouteValuesTagHelper<T>(IUrlHelperFactory urlHelperFactory, IHtmlGenerator htmlGenerator) : TagHelper
    where T : notnull, G4mvcBaseRouteValues
{
    private const string _anchor = "a";
    private const string _form = "form";
    private const string _attributeName = "g4-page";
    private const string _antiforgeryAttributeName = "asp-antiforgery";

    private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;
    private readonly IHtmlGenerator _htmlGenerator = htmlGenerator;

    [HtmlAttributeName(_attributeName)]
    public T RouteValues { get; set; } = null!;

    [HtmlAttributeName(_antiforgeryAttributeName)]
    public bool? Antiforgery { get; set; }

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(_attributeName);

        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        switch (output.TagName)
        {
            case _anchor:
                output.Attributes.SetAttribute("href", urlHelper.RouteUrl(RouteValues));
                await PostProcessAnchorTagAsync(context, output);
                break;
            case _form:
                output.Attributes.SetAttribute("action", urlHelper.RouteUrl(RouteValues));

                if (!Antiforgery.HasValue)
                {
                    var antiforgeryTag = _htmlGenerator.GenerateAntiforgery(ViewContext);
                    if (antiforgeryTag != null)
                    {
                        output.PostContent.AppendHtml(antiforgeryTag);
                    }
                }

                await PostProcessFormTagAsync(context, output);

                break;
            default:
                break;
        }

        await base.ProcessAsync(context, output);
    }

    public virtual Task PostProcessAnchorTagAsync(TagHelperContext context, TagHelperOutput output)
    {
        return Task.CompletedTask;
    }

    public virtual Task PostProcessFormTagAsync(TagHelperContext context, TagHelperOutput output)
    {
        return Task.CompletedTask;
    }
}

#endif