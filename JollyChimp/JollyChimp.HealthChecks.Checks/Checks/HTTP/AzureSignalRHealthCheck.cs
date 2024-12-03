using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.HTTP;

internal sealed class AzureSignalRHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure SignalR Hub Endpoint",
        Catergory = HealthCheckCategoryConstants.Http,
        IsEnabled = true,
        Type = HealthCheckType.AzureSignalR,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("URI", "SignalR Hub Endpoint")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("ACCESS_TOKEN", "SignalR Hub Endpoint AccessToken")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var endpointUri = config.Parameters.ExtractRequiredParameter("URI");
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var temporaryAccessToken = config.Parameters.ExtractRequiredParameter("ACCESS_TOKEN");
        
        builder.AddSignalRHub(
            hubConnectionBuilderFactory: serviceProvider =>
            {
                return () =>
                {
                    return new HubConnectionBuilder()
                        .WithUrl(endpointUri, options =>
                        {
                            options.AccessTokenProvider = () => Task.FromResult(temporaryAccessToken);
                        })
                        .WithAutomaticReconnect()
                        .Build();
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