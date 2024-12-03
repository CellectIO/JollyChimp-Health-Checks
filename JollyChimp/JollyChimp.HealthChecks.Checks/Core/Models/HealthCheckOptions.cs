using JollyChimp.Core.Common.Enums;

namespace JollyChimp.HealthChecks.Checks.Core.Models;

public sealed class HealthCheckOptions
{
    public required List<HealthCheckDefinition> Definitions { get; set; }
    public required List<HealthCheckType> Types { get; set; }
}