using JollyChimp.HealthChecks.Checks.Core.Models;
using JollyChimp.HealthChecks.WebHooks.Core.Models;

namespace JollyChimp.Core.Services.Core.Contracts.Services;

public interface IJollyDefinitionService
{
    IList<string> GetDeployedHealthCheckNames();
    IList<HealthCheckDefinition> GetHealthCheckDefinitions();
    List<string> GetDeployedEndpoints();
    IList<string> GetDeployedWebHooks();
    IList<WebHookDefinition> GetWebHooksDefinitions();
}