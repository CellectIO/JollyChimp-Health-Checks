using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.ServerlessFunctions;

internal sealed class AzureFunctionAppHttpEndpointHealthCheck : IJollyCheck
{

    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure Function App (http endpoint)",
        Catergory = HealthCheckCategoryConstants.ServerlessFunctions,
        IsEnabled = true,
        Type = HealthCheckType.AzureFunctionAppHttpEndpoint,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("URI", "HTTP API Endpoint")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("X_FUNCTION_KEY", "Function App Admin Security Key")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .HeaderTags("HEADERS", "Custom Headers (optional)")
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var endpoint = config.Parameters.ExtractRequiredParameter("URI");
        var adminAuthKey = config.Parameters.ExtractRequiredParameter("X_FUNCTION_KEY");
        var customHeaders = config.Parameters.ExtractOptionalParameter("HEADERS");
        
        builder.AddUrlGroup(
            uri: new Uri(endpoint),
            configureClient: (serviceProvider, client) =>
            {
                client.DefaultRequestHeaders.Add("x-functions-key", adminAuthKey);
                
                var headers = customHeaders.ExtractKeyValueTags();
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            },
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}
