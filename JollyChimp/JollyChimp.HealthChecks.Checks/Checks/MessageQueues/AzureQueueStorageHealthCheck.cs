using Azure.Storage.Queues;
using HealthChecks.Azure.Storage.Queues;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.MessageQueues;

internal sealed class AzureQueueStorageHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure Storage Queue",
        Catergory = HealthCheckCategoryConstants.MessageQueues,
        IsEnabled = true,
        Type = HealthCheckType.AzureQueueStorage,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CONNECTION_STRING", "Azure Storage Account Connection String")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("QUEUE_NAME", "Azure Queue Name")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var connectionString = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING");
        var queueName = config.Parameters.ExtractRequiredParameter("QUEUE_NAME");
        
        builder.AddAzureQueueStorage(
            clientFactory: sp =>
            {
                return new QueueServiceClient(connectionString);
            },
            optionsFactory: sp =>
            {
                return new AzureQueueStorageHealthCheckOptions
                {
                    QueueName = queueName
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