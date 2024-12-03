using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.Core.Data.Startup;
using JollyChimp.Core.Services.Startup;
using JollyChimp.HealthChecks.Checks.Startup;
using JollyChimp.HealthChecks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JollyChimp.HealthChecks.Startup;

public static class HealthChecksStartup
{
    public static WebApplicationBuilder RegisterJollyChimp(this WebApplicationBuilder webApplicationBuilder, HealthChecksStartupConfig config)
    {
        webApplicationBuilder
            .AddJollyChimpExternalDependencies()
            .AddJollyChimpHealthChecks(config)
            .RegisterJollyMonkeyHealthChecksUi(config)
            .RegisterJollyChimpData()
            .RegisterJollyChimpServices();

        return webApplicationBuilder;
    }

    public static IApplicationBuilder MapJollyChimp(this IApplicationBuilder builder, HealthChecksStartupConfig config)
    {
        builder.MapJollyMonkeyHealthChecksUi(config);

        return builder;
    }
    
    private static WebApplicationBuilder AddJollyChimpHealthChecks(this WebApplicationBuilder webApplicationBuilder, HealthChecksStartupConfig config)
    {
        var hcBuilder = webApplicationBuilder.Services
            .AddHealthChecks()
            .RegisterJollyMonkeyChecks(config.Checks);

        return webApplicationBuilder;
    }
    
    private static WebApplicationBuilder AddJollyChimpExternalDependencies(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddHttpClient();

        return webApplicationBuilder;
    }

}