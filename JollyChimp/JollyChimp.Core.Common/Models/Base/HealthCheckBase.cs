using Microsoft.Extensions.Diagnostics.HealthChecks;
using JollyChimp.Core.Common.Enums;

namespace JollyChimp.Core.Common.Models.Base;

public abstract class HealthCheckBase<TParamType>
{
    public string Name { get; set; }

    public HealthCheckType Type { get; set; }

    public HealthStatus FailureStatus { get; set; }

    public bool IsEnabled { get; set; }

    public List<string> Tags { get; set; }

    public List<TParamType> Parameters { get; set; }
}