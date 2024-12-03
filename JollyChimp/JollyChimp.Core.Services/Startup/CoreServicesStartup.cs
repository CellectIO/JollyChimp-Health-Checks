using JollyChimp.Core.Services.Core.Contracts.Services;
using JollyChimp.Core.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JollyChimp.Core.Services.Startup;

public static class CoreServicesStartup
{
    
    public static WebApplicationBuilder RegisterJollyChimpServices(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services
            .AddTransient<IExpressionValidationService, ExpressionValidationService>()
            .AddTransient<IEndPointService, EndPointService>()
            .AddTransient<ISettingsService, SettingsService>()
            .AddTransient<IWebHooksService, WebHooksService>()
            .AddTransient<IHealthChecksService, HealthChecksService>()
            .AddTransient<IJollyDefinitionService, JollyDefinitionService>();

        return webApplicationBuilder;
    }
    
}