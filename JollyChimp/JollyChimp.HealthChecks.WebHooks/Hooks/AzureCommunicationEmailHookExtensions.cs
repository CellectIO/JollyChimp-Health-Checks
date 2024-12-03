using Azure.Communication.Email;
using Azure;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using JollyChimp.HealthChecks.Templates.Services.TemplateGenerators;
using JollyChimp.Core.Common.Constants.Media;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.WebHooks.Core.Contracts;
using JollyChimp.HealthChecks.WebHooks.Core.Models;
using JollyChimp.HealthChecks.WebHooks.Extensions;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.WebHooks.Hooks;

internal sealed class AzureCommunicationEmailHookExtensions : IJollyHook
{

    public string Endpoint => "/Hooks/AzureCommunicationEmail";
    
    public WebHookDefinition Definition => new()
    {
        Name = "Azure Communication Email Services",
        Type = WebHookTypes.AzureCommunicationEmailHook,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Text("SHOULD_NOTIFY_WHEN_EXPRESSION", "Should notify when (Expression)")
                .WithRules("required", "hc_validators_webHookPredicate"),
            ParamDefinitionExtensions
                .Number("FAILURES_IN_MESSAGE_COUNT", "Failures in message count")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("COMMUNICATION_EMAILER_CONNECTIONSTRING", "Connection String")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("SENDER_ADDRESS", "Sender Address")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Tags("TO_ADDRESSES", "Receiver Addresses")
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
        var communicationConnectionString = webHookConfig.Parameters.ExtractRequiredParameter("COMMUNICATION_EMAILER_CONNECTIONSTRING");
        var senderAddress = webHookConfig.Parameters.ExtractRequiredParameter("SENDER_ADDRESS");
        var toAddresses = webHookConfig.Parameters.ExtractRequiredParameter("TO_ADDRESSES");
        
        endpointRouteBuilder.MapPost(Endpoint, async (
            [FromBody] WebHookPostRequest request
        ) =>
        {
            var content = request.Deserialize();
            var isOnline = content.IsOnline;

            var contentResponse = isOnline
                ? EmailHookTemplateGenerator.Success(_ =>
                {
                    _.ActivityImg = JollyChimpContentConstants.Gifs.JollyChimpStare;
                    _.RestoredResource = content.LivenessName;
                    _.ViewDashboardUrl = jollyUiEndpoint;
                })
                : EmailHookTemplateGenerator.Error(_ =>
                {
                    _.ImageUrl = JollyChimpContentConstants.Gifs.JollyChimpInform;
                    _.Services = content.Report.Report.Entries.Take(int.Parse(failuresInMessageCount)).ToDictionary();
                    _.ViewFailuresUrl = jollyUiEndpoint;
                    _.FailureResource = content.LivenessName;
                    _.FailureCount = content.FailureCount.ToString()!;
                });

            var emailHeading = isOnline ? 
                $"Health Checks are restored in : {content.LivenessName}" :
                $"Health Checks are failing in : {content.LivenessName}";

            var recipients = toAddresses
                .Split(',')
                .Select(_ => new EmailAddress(_))
                .ToList();

            var emailClient = new EmailClient(communicationConnectionString);
            var emailMessage = new EmailMessage(
                senderAddress: senderAddress,
                content: new EmailContent(emailHeading)
                {
                    PlainText = emailHeading,
                    Html = contentResponse
                },
                recipients: new EmailRecipients(recipients));

            EmailSendOperation emailSendOperation = emailClient.Send(
                WaitUntil.Completed,
                emailMessage);
        });

        return endpointRouteBuilder;
    }

}