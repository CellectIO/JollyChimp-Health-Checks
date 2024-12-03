using HealthChecks.UI.Core;

namespace JollyChimp.Core.Common.Models.HealthChecks;

public class WebHookHealthChecksReport
{

    public string Name { get; set; }

    public UIHealthReport Report { get; set; }

    public WebHookHealthChecksReport(string name, UIHealthReport report)
    {
        Name = name;
        Report = report;
    }

}