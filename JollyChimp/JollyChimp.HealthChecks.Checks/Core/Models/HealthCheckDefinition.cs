using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.HealthChecks.Checks.Core.Models;

public class HealthCheckDefinition
{
    public required string Name { get; set; }
    
    public required HealthCheckCategory Catergory { get; set; }

    public required bool IsEnabled { get; set; }
    
    public required HealthCheckType Type { get; set; }

    public required List<ParameterDefinition> Parameters { get; set; }
}