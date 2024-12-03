namespace JollyChimp.Core.Common.Enums;

public enum HealthCheckType
{
    SqlServerDatabase = 0,
    SqlServerDatabaseQuery = 1,
    AzureFunctionAppHttpEndpoint = 2,
    AzureKeyVault = 3,
    AzureSignalR = 4,
    Redis = 5,
    AzureApplicationInsights = 6,
    HttpEndpointUnProtected = 7,
    AzureQueueStorage = 8,
    AzureFileShare = 9,
    AzureBlobStorage = 10,
    OpenIdConnectServer = 11,
    AzureServiceBusQueue = 12,
    AzureServiceBusTopic = 13,
    AzureServiceBusSubscription = 14,
    AzureServiceBusQueueMessageCountThreshold = 15,
    RabbitMq = 16,
    NpqSqlDatabase = 17,
    NpqSqlDatabaseQuery = 18,
    MongoDb = 19
}