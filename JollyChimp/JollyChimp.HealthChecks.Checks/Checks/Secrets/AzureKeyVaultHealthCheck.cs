using Azure.Identity;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Secrets;

internal sealed class AzureKeyVaultHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure KeyVault",
        Catergory = HealthCheckCategoryConstants.Secrets,
        IsEnabled = true,
        Type = HealthCheckType.AzureKeyVault,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Url("URI", "Azure KeyVault uri")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Tags("CERTIFICATES", "Certificates"),
            ParamDefinitionExtensions
                .Tags("KEYS", "Keys"),
            ParamDefinitionExtensions
                .Tags("SECRETS", "Secrets")
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var endpointUri = config.Parameters.ExtractRequiredParameter("URI");
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var certsParams = config.Parameters.ExtractOptionalParameter("CERTIFICATES");
        var keysParams = config.Parameters.ExtractOptionalParameter("KEYS");
        var secretsParams = config.Parameters.ExtractOptionalParameter("SECRETS");

        if (certsParams == null && keysParams == null && secretsParams == null)
        {
            throw new ArgumentException("Need To provide at least 1 of the following: certificate, key or secret");
        }
        
        builder.AddAzureKeyVault(
            keyVaultServiceUri: new Uri(endpointUri),
            credential: new DefaultAzureCredential(), //TODO: this should be an parameter too.
            setup: options =>
            {
                foreach (var cert in certsParams.ExtractTags())
                {
                    options.AddCertificate(cert, true);
                }
                foreach (var cert in  keysParams.ExtractTags())
                {
                    options.AddKey(cert);
                }
                foreach (var secret in  secretsParams.ExtractTags())
                {
                    options.AddKey(secret);
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