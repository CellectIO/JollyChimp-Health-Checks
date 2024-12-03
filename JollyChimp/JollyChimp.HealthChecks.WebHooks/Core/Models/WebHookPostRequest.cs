using JollyChimp.Core.Common.Models.HealthChecks;
using System.Text.Json;

namespace JollyChimp.HealthChecks.WebHooks.Core.Models;

internal class WebHookPostRequest : DefaultJollyChimpApiHookBase<string>
{

    public override bool IsOnline => string.IsNullOrWhiteSpace(Report);
    
    public override int? FailureCount => null;

    public JollyChimpWebHookParsedRequest Deserialize()
    {
        var response = new JollyChimpWebHookParsedRequest()
        {
            LivenessName = LivenessName
        };

        if (!string.IsNullOrWhiteSpace(Report))
        {
            response.Report = JsonSerializer.Deserialize<WebHookHealthChecksReport>(Report);
        }

        return response;
    }
}

internal class JollyChimpWebHookParsedRequest : DefaultJollyChimpApiHookBase<WebHookHealthChecksReport>
{
    public override bool IsOnline => Report == null && ((Report?.Report?.Entries.Count ?? 0) <= 0);
    public override int? FailureCount => Report?.Report?.Entries?.Count ?? 0;

}

internal abstract class DefaultJollyChimpApiHookBase<TReport>
{
    public string LivenessName { get; set; }

    public TReport? Report { get; set; }

    public abstract bool IsOnline { get; }

    public abstract int? FailureCount { get; }

}