using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JollyChimp.HealthChecks.Checks.Core.Contracts;

internal interface IJollyCheck
{
    
    public HealthCheckDefinition Definition { get; }
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config);
}