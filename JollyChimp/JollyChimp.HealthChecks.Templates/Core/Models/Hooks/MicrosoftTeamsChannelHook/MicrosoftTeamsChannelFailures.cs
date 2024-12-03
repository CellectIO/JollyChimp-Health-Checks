using HealthChecks.UI.Core;
using JollyChimp.Core.Common.Models.HealthChecks;

namespace JollyChimp.HealthChecks.Templates.Core.Models.Hooks.MicrosoftTeamsChannelHook;

public sealed class MicrosoftTeamsChannelFailures
{
    public string FailureResource { get; set; }
    public string FailureCount { get; set; }
    public string ActivityImage { get; set; }
    public string ViewFailuresUrl { get; set; }
    public Dictionary<string, UIHealthReportEntry> Services { get; set; }
}
