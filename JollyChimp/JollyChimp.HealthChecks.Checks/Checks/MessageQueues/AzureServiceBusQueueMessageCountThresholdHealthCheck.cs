using Azure.Identity;
using HealthChecks.AzureServiceBus.Configuration;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.MessageQueues;

internal sealed class AzureServiceBusQueueMessageCountThresholdHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure Service Bus Queue Message Count Threshold",
        Catergory = HealthCheckCategoryConstants.MessageQueues,
        IsEnabled = true,
        Type = HealthCheckType.AzureServiceBusQueueMessageCountThreshold,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("ENDPOINT", "Endpoint")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("QUEUE_NAME", "Queue name")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Number("ACTIVE_MESSAGES_UNHEALTHY_THRESHOLD", "Active Messages: unhealthy threshold")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Number("ACTIVE_MESSAGES_DEGRADED_THRESHOLD", "Active Messages: degraded threshold")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Number("DEAD_LETTER_MESSAGES_UNHEALTHY_THRESHOLD", "Dead Letter Messages: unhealthy threshold")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Number("DEAD_LETTER_MESSAGES_DEGRADED_THRESHOLD", "Dead Letter Messages: degraded threshold")
                .WithRequiredRule(),
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var endpoint = config.Parameters.ExtractRequiredParameter("ENDPOINT");
        var queueName = config.Parameters.ExtractRequiredParameter("QUEUE_NAME");
        var activateMessagesUnhealthyThreshold = config.Parameters.ExtractRequiredParameter("ACTIVE_MESSAGES_UNHEALTHY_THRESHOLD");
        var activateMessagesDegradedThreshold = config.Parameters.ExtractRequiredParameter("ACTIVE_MESSAGES_DEGRADED_THRESHOLD");
        var deadLetterMessagesUnhealthyThreshold = config.Parameters.ExtractRequiredParameter("DEAD_LETTER_MESSAGES_UNHEALTHY_THRESHOLD");
        var deadLetterMessagesDegradedThreshold = config.Parameters.ExtractRequiredParameter("DEAD_LETTER_MESSAGES_DEGRADED_THRESHOLD");
        
        builder.AddAzureServiceBusQueueMessageCountThreshold(
            endpoint: endpoint,
            queueName: queueName,
            tokenCredential: new DefaultAzureCredential(), //TODO: THIS NEEDS TO BE AN PARAMETER
            configure: options =>
            {
                options.ActiveMessages = new AzureServiceBusQueueMessagesCountThreshold()
                {
                    UnhealthyThreshold = int.Parse(activateMessagesUnhealthyThreshold),
                    DegradedThreshold = int.Parse(activateMessagesDegradedThreshold)
                };
                options.DeadLetterMessages = new AzureServiceBusQueueMessagesCountThreshold()
                {
                    UnhealthyThreshold = int.Parse(deadLetterMessagesUnhealthyThreshold),
                    DegradedThreshold = int.Parse(deadLetterMessagesDegradedThreshold)
                };
            },
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}