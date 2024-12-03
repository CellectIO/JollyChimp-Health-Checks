using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.HTTP;

internal sealed class HttpEndpointUnProtectedHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "HTTP Endpoint (public)",
        Catergory = HealthCheckCategoryConstants.Http,
        IsEnabled = true,
        Type = HealthCheckType.HttpEndpointUnProtected,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("URI", "Uri")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .HeaderTags("HEADERS", "Custom Headers (optional)")
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var endpoint = config.Parameters.ExtractRequiredParameter("URI");
        var customHeaders = config.Parameters.ExtractOptionalParameter("HEADERS");
        
        builder.AddUrlGroup(
            uri: new Uri(endpoint),
            configureClient: (provider, client) =>
            {
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