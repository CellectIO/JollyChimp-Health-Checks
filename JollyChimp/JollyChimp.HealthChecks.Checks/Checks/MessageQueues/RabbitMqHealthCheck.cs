using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.MessageQueues;

internal sealed class RabbitMqHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "RabbitMq",
        Catergory = HealthCheckCategoryConstants.MessageQueues,
        IsEnabled = false,
        Type = HealthCheckType.RabbitMq,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("CONNECTION_STRING_URI", "Connection String Endpoint")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("SERVER_NAME", "Server Name")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("CERTIFICATE_PATH", "Certificate Path")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .CheckBox("SSL_ENABLED", "SSL enabled")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var endpoint = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING_URI");
        var serverName = config.Parameters.ExtractRequiredParameter("SERVER_NAME");
        var certificatePath = config.Parameters.ExtractRequiredParameter("CERTIFICATE_PATH");
        var isEnabled = config.Parameters.ExtractRequiredParameter("SSL_ENABLED");
        
        builder.AddRabbitMQ(
            rabbitConnectionString: new Uri(endpoint),
            sslOption: new SslOption(
                serverName: serverName,
                certificatePath: certificatePath,
                enabled: bool.Parse(isEnabled)
                ),
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}