using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace G4mvc.Test_9.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    [BindProperty]
    public TestFormModel? TestModel { get; set; }

    public void OnGet()
    {
    }

    public void OnPost()
    {
    }

    public void OnPostAlternative()
    {
    }
}

public record TestFormModel(string Name, string Description);
