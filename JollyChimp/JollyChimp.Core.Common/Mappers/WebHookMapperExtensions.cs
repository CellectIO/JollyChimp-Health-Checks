using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.Core.Common.Mappers;

public static class WebHookMapperExtensions
{
    public static WebHookStartupConfig MapToWebHookStartupConfig(this WebHookEntity hook) =>
        new()
        {
            Name = hook.Name,
            IsEnabled = hook.IsEnabled,
            Type = hook.Type,
            Parameters = hook.WebHookParameters.MapToParameterStartupConfigList()
        };

    public static List<WebHookStartupConfig> MapToWebHookStartupConfigList(this IEnumerable<WebHookEntity> hooks) =>
        hooks
            .Select(MapToWebHookStartupConfig)
            .ToList();
    
    public static WebHookApiResponse MapToWebHookApiResponse(this WebHookEntity hook, bool isDeployed) =>
        new()
        {
            ID = hook.ID,
            Name = hook.Name,
            IsEnabled = hook.IsEnabled,
            Type = hook.Type,
            IsDeployed = isDeployed,
            Parameters = hook.WebHookParameters.MapToParameterApiResponseList()
        };

    public static WebHookEntity MapToNewWebHookEntity(this UpsertWebHookRequest request) =>
        new()
        {
            Name = request.Name,
            Type = request.Type,
            IsEnabled = request.IsEnabled,
            IsDeleted = false,
            WebHookParameters = request.Parameters.MapToWebHookParameterEntityList()
        };
    
    public static WebHookEntity MapToExistingWebHookEntity(this UpsertWebHookRequest request) =>
        new()
        {
            ID = request.ID.Value,
            Name = request.Name,
            Type = request.Type,
            IsEnabled = request.IsEnabled,
            IsDeleted = false,
            WebHookParameters = request.Parameters.MapToWebHookParameterEntityList()
        };
}