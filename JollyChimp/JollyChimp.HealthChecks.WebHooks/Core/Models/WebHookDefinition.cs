using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.HealthChecks.WebHooks.Core.Models;

public class WebHookDefinition
{
    public string Name { get; set; }

    public WebHookTypes Type { get; set; }

    public List<ParameterDefinition> Parameters { get; set; }
}