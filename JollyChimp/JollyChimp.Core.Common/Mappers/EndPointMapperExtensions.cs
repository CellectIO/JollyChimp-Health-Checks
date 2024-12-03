using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.Core.Common.Mappers;

public static class EndPointMapperExtensions
{
    public static EndPointStartupConfig MapToEndPointStartupConfig(this EndPointEntity endpoint) => 
        new()
        {
            Name = endpoint.Name,
            ApiPath = endpoint.ApiPath,
            HealthChecksPredicate = endpoint.HealthChecksPredicate,
            IsEnabled = endpoint.IsEnabled
        };
    
    public static List<EndPointStartupConfig> MapToEndPointStartupConfigList(this IEnumerable<EndPointEntity> endpoints) => 
        endpoints
            .Select(MapToEndPointStartupConfig)
            .ToList();
    
    public static EndpointApiResponse MapToEndpointApiResponse(this EndPointEntity endpoint, bool isDeployed) => 
        new()
        {
            ID = endpoint.ID,
            Name = endpoint.Name,
            ApiPath = endpoint.ApiPath,
            HealthChecksPredicate = endpoint.HealthChecksPredicate,
            IsEnabled = endpoint.IsEnabled,
            IsDeployed = isDeployed
        };
    
    public static EndPointEntity MapToNewEndPointEntity(this UpsertEndPointRequest endpoint) => 
        new()
        {
            Name = endpoint.Name,
            ApiPath = endpoint.ApiPath,
            HealthChecksPredicate = endpoint.HealthChecksPredicate,
            IsEnabled = endpoint.IsEnabled
        };
    
    public static EndPointEntity MapToExistingEndPointEntity(this UpsertEndPointRequest endpoint)
    {
        var response = endpoint.MapToNewEndPointEntity();
        response.ID = endpoint.ID.Value;
        return response;
    }
}