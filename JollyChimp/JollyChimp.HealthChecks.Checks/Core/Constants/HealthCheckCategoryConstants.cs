using JollyChimp.HealthChecks.Checks.Core.Models;

namespace JollyChimp.HealthChecks.Checks.Core.Constants;

internal static class HealthCheckCategoryConstants
{
    public static readonly HealthCheckCategory Auth = new("Authorization", 1);
    public static readonly HealthCheckCategory Secrets = new("Secrets", 1);
    public static readonly HealthCheckCategory Http = new("HTTP", 2);
    public static readonly HealthCheckCategory Storage = new("Storage", 3);
    public static readonly HealthCheckCategory Logging = new("Logging", 3);
    public static readonly HealthCheckCategory ServerlessFunctions = new("Serverless Functions", 4);
    public static readonly HealthCheckCategory MessageQueues = new("Message Queues", 5);
}