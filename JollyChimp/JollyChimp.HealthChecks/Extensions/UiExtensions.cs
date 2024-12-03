using HealthChecks.UI.Configuration;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.Core.Data.Startup;
using JollyChimp.HealthChecks.WebHooks.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JollyChimp.HealthChecks.Extensions;

internal static class UiExtensions
{
    
    public static WebApplicationBuilder RegisterJollyMonkeyHealthChecksUi(
        this WebApplicationBuilder webApplicationBuilder, 
        HealthChecksStartupConfig config)
    {
        webApplicationBuilder.Services.RegisterWebHookOptions();
        webApplicationBuilder.Services.AddHealthChecksUI(settings =>
        {
            settings.ConfigureUiSettings(config);
            settings.ConfigureUiEndpoints(config);
            settings.RegisterJollyChimpWebHooks(config);
        }).AddJollyChimpSqlServerStorage(config.Server.SqlServerConnectionString); //TODO: THIS SHOULD PROBABLY BE MOVED SOMEWHERE ELSE

        return webApplicationBuilder;
    }

    private static Settings ConfigureUiSettings(this Settings settings, HealthChecksStartupConfig config)
    {
        var uiConfig = config.UI;

        settings.SetEvaluationTimeInSeconds(uiConfig.EvaluationTimeInSeconds);
        settings.MaximumHistoryEntriesPerEndpoint(uiConfig.MaximumHistoryEntriesPerEndpoint);
        settings.SetApiMaxActiveRequests(uiConfig.ApiMaxActiveRequests);
        settings.SetHeaderText(uiConfig.HeaderText);
        settings.SetMinimumSecondsBetweenFailureNotifications(uiConfig.MinimumSecondsBetweenFailureNotifications);

        if (uiConfig.NotifyUnHealthyOneTimeUntilChange)
        {
            settings.SetNotifyUnHealthyOneTimeUntilChange();
        }

        return settings;
    }
    
    private static Settings ConfigureUiEndpoints(this Settings settings, HealthChecksStartupConfig config)
    {
        foreach (var endpoint in config.EndPoints)
        {
            settings.AddHealthCheckEndpoint(endpoint.Name, endpoint.ApiPath);
        }

        return settings;
    }
    
}