using JollyChimp.Core.Common.Enums;

namespace JollyChimp.HealthChecks.WebHooks.Core.Models;

public sealed class WebHookOptions
{
    public required List<WebHookDefinition> Definitions { get; set; }
    public required List<WebHookTypes> Types { get; set; }
}