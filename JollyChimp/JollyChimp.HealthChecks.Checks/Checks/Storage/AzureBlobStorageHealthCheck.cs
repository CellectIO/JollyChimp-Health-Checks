using Azure.Storage.Blobs;
using HealthChecks.Azure.Storage.Blobs;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Storage;

internal sealed class AzureBlobStorageHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure Blob Storage",
        Catergory = HealthCheckCategoryConstants.Storage,
        IsEnabled = true,
        Type = HealthCheckType.AzureBlobStorage,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CONNECTION_STRING", "Azure Storage Account Connection String")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("CONTAINER_NAME", "Container Name")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var connectionString = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING");
        var containerName = config.Parameters.ExtractRequiredParameter("CONTAINER_NAME");
        
        builder.AddAzureBlobStorage(
            clientFactory: sp =>
            {
                return new BlobServiceClient(connectionString);
            },
            optionsFactory: sp =>
            {
                return new AzureBlobStorageHealthCheckOptions()
                {
                    ContainerName = containerName
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