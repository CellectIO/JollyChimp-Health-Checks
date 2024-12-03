using HealthChecks.UI.Core;
using JollyChimp.HealthChecks.Templates.Core.Constants;

namespace JollyChimp.HealthChecks.Templates.Core.Models.Hooks.EmailHook;

public sealed class EmailFailures
{
    public string FailureResource { get; set; }
    public string FailureCount { get; set; }
    public string ImageUrl { get; set; }
    public string ViewFailuresUrl { get; set; }

    public Dictionary<string, UIHealthReportEntry> Services { get; set; }

    public ThemeConstants Themes { get; set; } = new ThemeConstants();
}
