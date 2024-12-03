using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.Core.Common.Mappers;

public static class HealthCheckMapperExtensions
{
    public static HealthCheckStartupConfig MapToHealthCheckStartupConfig(this HealthCheckEntity check) =>
        new()
        {
            Name = check.Name,
            IsEnabled = check.IsEnabled,
            Type = check.Type,
            FailureStatus = check.HealthStatus,
            Tags = check.Tags.Split(',').ToList(),
            Parameters = check.HealthCheckParameters.MapToParameterStartupConfigList()
        };

    public static List<HealthCheckStartupConfig> MapToHealthCheckStartupConfigList(this IEnumerable<HealthCheckEntity> checks) =>
        checks
            .Select(MapToHealthCheckStartupConfig)
            .ToList();
    
    public static HealthCheckApiResponse MapToHealthCheckApiResponse(this HealthCheckEntity check, bool isDeployed) =>
        new()
        {
            ID = check.ID,
            Name = check.Name,
            IsEnabled = check.IsEnabled,
            Type = check.Type,
            FailureStatus = check.HealthStatus,
            IsDeployed = isDeployed,
            Tags = check.Tags.Split(',').ToList(),
            Parameters = check.HealthCheckParameters.MapToParameterApiResponseList()
        };

    public static HealthCheckEntity MapToNewHealthCheckEntity(this UpsertHealthCheckRequest request) =>
        new()
        {
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            Type = request.Type,
            HealthStatus = request.FailureStatus,
            Tags = string.Join(",", request.Tags),
            HealthCheckParameters = request.Parameters.MapToHealthCheckParameterEntityList()
        };
    
    public static HealthCheckEntity MapToExistingHealthCheckEntity(this UpsertHealthCheckRequest request) =>
        new()
        {
            ID = request.ID.Value,
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            Type = request.Type,
            HealthStatus = request.FailureStatus,
            Tags = string.Join(",", request.Tags),
            HealthCheckParameters = request.Parameters.MapToHealthCheckParameterEntityList()
        };
}