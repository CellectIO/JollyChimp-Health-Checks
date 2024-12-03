using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using JollyChimp.Core.Common.Constants.Media;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Templates.Services.TemplateGenerators;
using System.Text;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.HealthChecks.WebHooks.Core.Contracts;
using JollyChimp.HealthChecks.WebHooks.Core.Models;
using JollyChimp.HealthChecks.WebHooks.Extensions;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.WebHooks.Hooks;

internal sealed class MicrosoftTeamsChannelHookExtensions : IJollyHook
{

    public string Endpoint => "/Hooks/MicrosoftTeamsChannel";
    
    public WebHookDefinition Definition => new()
    {
        Name = "Microsoft Teams Channel",
        Type = WebHookTypes.MicrosoftTeamsChannelHook,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Text("SHOULD_NOTIFY_WHEN_EXPRESSION", "Should notify when (Expression)")
                .WithRules("required", "hc_validators_webHookPredicate"),
            ParamDefinitionExtensions
                .Number("FAILURES_IN_MESSAGE_COUNT", "Failures in message count")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("TEAMS_ENDPOINT", "Microsoft Teams Connector endpoint")
                .WithRequiredRule()
        }
    };

    public Settings RegisterHook(
        Settings uiSettings, 
        WebHookStartupConfig webHookConfig,
        HealthChecksStartupConfig hcConfig
        )
    {
        return uiSettings.RegisterHook(webHookConfig, Endpoint);
    }

    public IEndpointRouteBuilder UseHook(
        IEndpointRouteBuilder endpointRouteBuilder, 
        WebHookStartupConfig webHookConfig,
        HealthChecksStartupConfig hcConfig
        )
    {
        if (!webHookConfig.IsEnabled)
        {
            return endpointRouteBuilder;
        }

        var jollyUiEndpoint = $"{hcConfig.Server.Domain}{hcConfig.UI.UiRoutePath}";
        var failuresInMessageCount = webHookConfig.Parameters.ExtractRequiredParameter("FAILURES_IN_MESSAGE_COUNT");
        var teamsEndpoint = webHookConfig.Parameters.ExtractRequiredParameter("TEAMS_ENDPOINT");
        
        endpointRouteBuilder.MapPost(Endpoint, async (
            [FromBody] WebHookPostRequest request,
            IHttpClientFactory httpClientFactory
            ) => {
                var content = request.Deserialize();

                var contentResponse = content.IsOnline
                    ? MicrosoftTeamsChannelHookTemplateGenerator.Success(_ =>
                    {
                        _.ActivityImg = JollyChimpContentConstants.Images.JollyChimpStare;
                        _.RestoredResource = content.LivenessName;
                        _.ViewDashboardUrl = jollyUiEndpoint;
                    })
                    : MicrosoftTeamsChannelHookTemplateGenerator.Error(_ =>
                    {
                        _.ActivityImage = JollyChimpContentConstants.Images.JollyChimpAlerted;
                        _.Services = content.Report.Report.Entries.Take(int.Parse(failuresInMessageCount)).ToDictionary();
                        _.ViewFailuresUrl = jollyUiEndpoint;
                        _.FailureResource = content.LivenessName;
                        _.FailureCount = content.FailureCount!.ToString()!;
                    });

                var client = httpClientFactory.CreateClient();

                var response = await client.PostAsync(
                    teamsEndpoint,
                    new StringContent(contentResponse, Encoding.UTF8, "application/json")
                    );

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
            });

        return endpointRouteBuilder;
    }

}
