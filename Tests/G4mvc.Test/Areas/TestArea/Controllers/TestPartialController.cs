using G4mvc.Test.Areas.TestArea.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace G4mvc.Test.Areas.TestArea.Controllers;

[Area("TestArea")]
public partial class TestPartialController(ILogger<TestController> logger) : Controller
{
    private readonly ILogger<TestController> _logger = logger;

#nullable disable
    [HttpGet]
#pragma warning disable IDE0060 // Remove unused parameter
    public IActionResult Index(string test)
        => View();
#pragma warning restore IDE0060 // Remove unused parameter
#nullable restore

    [HttpGet]
    public IActionResult Privacy()
        => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred");
        return View(new TestAreaErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
