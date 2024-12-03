using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Auth;

internal sealed class OpenIdConnectServerHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "OpenId Connect Server",
        Catergory = HealthCheckCategoryConstants.Auth,
        IsEnabled = true,
        Type = HealthCheckType.OpenIdConnectServer,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("URI", "OpenID Connect Server HTTP endpoint")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("DISCOVERY_CONFIG_SEGMENT", "Identity Server discover configuration segment")
                .WithRequiredRule(),
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var uri = config.Parameters.ExtractRequiredParameter("URI");
        var discoveryConfigSegment = config.Parameters.ExtractRequiredParameter("DISCOVERY_CONFIG_SEGMENT");
        
        builder.AddIdentityServer(
            idSvrUri: new Uri(uri),
            discoverConfigurationSegment: discoveryConfigSegment,
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}