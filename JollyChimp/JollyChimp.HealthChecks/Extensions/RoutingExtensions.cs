using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.WebHooks.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JollyChimp.HealthChecks.Extensions;

internal static class RoutingExtensions
{
    
    public static IApplicationBuilder MapJollyMonkeyHealthChecksUi(this IApplicationBuilder builder, HealthChecksStartupConfig config)
    {
        builder.UseEndpoints(endpoints =>
        {
            endpoints.ConfigureEndpoints(config.EndPoints);
            endpoints.MapHealthChecksUI(options =>
            {
                options.ConfigureHealthChecksUi(config.UI);
            });
            endpoints.UseJollyChimpWebHooks(config);
        });

        return builder;
    }

    private static Options ConfigureHealthChecksUi(this Options options, UIStartupConfig config)
    {
        options.UIPath = config.UiRoutePath;
        
        //REGISTER UI STYLES
        options.AddCustomStylesheet($"{AppDomain.CurrentDomain.BaseDirectory}wwwroot\\css\\theme-vars.css");
        options.AddCustomStylesheet($"{AppDomain.CurrentDomain.BaseDirectory}wwwroot\\css\\health-check-dashboard.css");

        return options;
    }
    
    private static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder builder, List<EndPointStartupConfig> endpoints)
    {
        var scriptingOptions = CSharpScriptingExtensions.GetAssemblyReferenceOptions(typeof(HealthCheckRegistration));
        
        foreach (var endpoint in endpoints.Where(_ => _.IsEnabled))
        {
            var func = CSharpScriptingExtensions
                .ParseEndPointPredicate(endpoint.HealthChecksPredicate, scriptingOptions);
            
            builder.MapHealthChecks(endpoint.ApiPath, new HealthCheckOptions
            {
                Predicate = func,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
        
        return builder;
    }
    
    
    
}