using JollyChimp.HealthChecks.Templates.Core.Constants;

namespace JollyChimp.HealthChecks.Templates.Core.Models.Hooks.EmailHook;

public sealed class EmailRestored
{
    public string RestoredResource { get; set; }
    public string ActivityImg { get; set; }
    public string ViewDashboardUrl { get; set; }
    public ThemeConstants Themes { get; set; } = new ();
}
