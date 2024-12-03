using Azure.Storage.Files.Shares;
using HealthChecks.Azure.Storage.Files.Shares;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Storage;

internal sealed class AzureFileShareHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure File Share",
        Catergory = HealthCheckCategoryConstants.Storage,
        IsEnabled = true,
        Type = HealthCheckType.AzureFileShare,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CONNECTION_STRING", "Azure Storage Account Connection String")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("SHARE_NAME", "Azure File Share Name")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var connectionString = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING");
        var shareName = config.Parameters.ExtractRequiredParameter("SHARE_NAME");
        
        builder.AddAzureFileShare(
            clientFactory: sp =>
            {
                return new ShareServiceClient(connectionString);
            },
            optionsFactory: sp =>
            {
                return new AzureFileShareHealthCheckOptions()
                {
                    ShareName = shareName
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