using Azure.Identity;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.MessageQueues;

internal sealed class AzureServiceBusSubscriptionHealthCheck : IJollyCheck
{
    public HealthCheckDefinition Definition => new()
    {
        Name = "Azure Service Bus Subscription",
        Catergory = HealthCheckCategoryConstants.MessageQueues,
        IsEnabled = true,
        Type = HealthCheckType.AzureServiceBusSubscription,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("FULLY_QUALIFIED_NAMESPACE", "Fully Qualified Namespace")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("TOPIC_NAME", "Topic Name")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("SUBSCRIPTION_NAME", "Subscription Name")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("TENANT_ID", "Tenant ID")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CLIENT_ID", "Client ID")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CLIENT_SECRET", "Client Secret")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var fullyQualifiedNamespace = config.Parameters.ExtractRequiredParameter("FULLY_QUALIFIED_NAMESPACE");
        var topicName = config.Parameters.ExtractRequiredParameter("TOPIC_NAME");
        var subscriptionName = config.Parameters.ExtractRequiredParameter("SUBSCRIPTION_NAME");
        var tenantId = config.Parameters.ExtractRequiredParameter("TENANT_ID");
        var clientId = config.Parameters.ExtractRequiredParameter("CLIENT_ID");
        var clientSecret = config.Parameters.ExtractRequiredParameter("CLIENT_SECRET");
        
        builder.AddAzureServiceBusSubscription(
            fullyQualifiedNamespaceFactory: sp => fullyQualifiedNamespace,
            topicNameFactory: sp => topicName,
            subscriptionNameFactory: sp => subscriptionName,
            tokenCredentialFactory: sp =>
            {
                // Configure the token credentials using Azure.Identity
                return new ClientSecretCredential(
                    tenantId: tenantId,
                    clientId: clientId,
                    clientSecret: clientSecret);
            },
            configure: options =>
            {
                
            },
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}