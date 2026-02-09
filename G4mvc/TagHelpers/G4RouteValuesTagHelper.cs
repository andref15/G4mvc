#if !NETSTANDARD
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.TagHelpers;

public abstract class G4RouteValuesTagHelper<T>(string attributeName, IUrlHelperFactory urlHelperFactory, IHtmlGenerator htmlGenerator) : TagHelper
    where T : G4mvcBaseRouteValues<T>
{
    private const int _formTagHelperOrder = -1000;
    private const string _antiforgeryAttributeName = "asp-antiforgery";

    protected const string Anchor = "a";
    protected const string Form = "form";

    private readonly string _attributeName = attributeName;
    private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;
    private readonly IHtmlGenerator _htmlGenerator = htmlGenerator;

    public override int Order => _formTagHelperOrder - 1;

    public abstract T RouteValues { get; set; }

    [HtmlAttributeNotBound, ViewContext]
    public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(_attributeName);

        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        switch (output.TagName)
        {
            case Anchor:
                output.Attributes.SetAttribute("href", urlHelper.RouteUrl(RouteValues));
                await PostProcessAnchorTagAsync(context, output);
                break;
            case Form:
                output.Attributes.SetAttribute("action", urlHelper.RouteUrl(RouteValues));

                if (!context.AllAttributes.ContainsName(_antiforgeryAttributeName))
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