using HealthChecks.UI.Configuration;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.WebHooks.Core.Models;
using Microsoft.AspNetCore.Routing;

namespace JollyChimp.HealthChecks.WebHooks.Core.Contracts;

internal interface IJollyHook
{
    
    public string Endpoint { get; }
    
    public WebHookDefinition Definition { get; }

    public Settings RegisterHook(
        Settings uiSettings,
        WebHookStartupConfig webHookConfig,
        HealthChecksStartupConfig hcConfig
    );

    public IEndpointRouteBuilder UseHook(
        IEndpointRouteBuilder endpointRouteBuilder,
        WebHookStartupConfig webHookConfig,
        HealthChecksStartupConfig hcConfig
    );

}