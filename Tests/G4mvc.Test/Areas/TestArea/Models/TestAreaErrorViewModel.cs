namespace G4mvc.Test.Areas.TestArea.Models;

public class TestAreaErrorViewModel
{
    public string RequestId { get; set; } = null!;

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
