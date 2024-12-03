using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.Core.Common.Mappers;

public static class ParameterMapperExtensions
{
    public static ParameterStartupConfig MapToParameterStartupConfig(this WebHookParameterEntity parameter) =>
        new()
        {
            Name = parameter.Name,
            Value = parameter.Value
        };
    
    public static ParameterStartupConfig MapToParameterStartupConfig(this HealthCheckParameterEntity parameter) =>
        new()
        {
            Name = parameter.Name,
            Value = parameter.Value
        };
    
    public static ParameterApiResponse MapToParameterApiResponse(this WebHookParameterEntity parameter) =>
        new()
        {
            Name = parameter.Name,
            Value = parameter.Value
        };
    
    public static ParameterApiResponse MapToParameterApiResponse(this HealthCheckParameterEntity parameter) =>
        new()
        {
            Name = parameter.Name,
            Value = parameter.Value
        };
    
    public static List<ParameterStartupConfig> MapToParameterStartupConfigList(this IEnumerable<WebHookParameterEntity> parameters) =>
        parameters
            .Select(MapToParameterStartupConfig)
            .ToList();
    
    public static List<ParameterStartupConfig> MapToParameterStartupConfigList(this IEnumerable<HealthCheckParameterEntity> parameters) =>
        parameters
            .Select(MapToParameterStartupConfig)
            .ToList();
    
    public static List<ParameterApiResponse> MapToParameterApiResponseList(this IEnumerable<WebHookParameterEntity> parameters) =>
        parameters
            .Select(MapToParameterApiResponse)
            .ToList();
    
    public static List<ParameterApiResponse> MapToParameterApiResponseList(this IEnumerable<HealthCheckParameterEntity> parameters) =>
        parameters
            .Select(MapToParameterApiResponse)
            .ToList();
    
    public static WebHookParameterEntity MapToWebHookParameterEntity(this ParameterApiResponse param)
        => new()
        {
            Name = param.Name,
            Value = param.Value,
            IsEnabled = true,
            IsDeleted = false
        };
    
    public static HealthCheckParameterEntity MapToHealthCheckParameterEntity(this ParameterApiResponse param)
        => new()
        {
            Name = param.Name,
            Value = param.Value,
            IsEnabled = true,
            IsDeleted = false
        };

    public static List<WebHookParameterEntity> MapToWebHookParameterEntityList(this IEnumerable<ParameterApiResponse> parameters)
        => parameters
            .Select(MapToWebHookParameterEntity)
            .ToList();
    
    public static List<HealthCheckParameterEntity> MapToHealthCheckParameterEntityList(this IEnumerable<ParameterApiResponse> parameters)
        => parameters
            .Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .Select(MapToHealthCheckParameterEntity)
            .ToList();

    public static Dictionary<string, string> ExtractKeyValueTags(this string? tagString)
    {
        var response = new Dictionary<string, string>();
        
        if (!string.IsNullOrEmpty(tagString))
        {
            var headers = tagString.Split(",");
            foreach (var tag in headers)
            {
                var headerDetails = tag.Split(":");
                response.Add(headerDetails[0], headerDetails[1]);
            }
        }

        return response;
    }
    
    public static List<string> ExtractTags(this string? tagString)
    {
        var response = new List<string>();

        if (!string.IsNullOrEmpty(tagString))
        {
            var headers = tagString.Split(",");
            foreach (var tag in headers)
            {
                response.Add(tag);
            }
        }

        return response;
    }


}