using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.Base;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.Core.Common.Seeds;

namespace JollyChimp.Core.Common.Mappers;

public static class ServerSettingsMapperExtensions
{
    
    public static UIStartupConfig MapToUIStartupConfig(this IEnumerable<ServerSettingEntity> settings) =>
        new()
        {
            EvaluationTimeInSeconds = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.EvaluationTimeInSeconds.ID).Value),
            ApiMaxActiveRequests = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.ApiMaxActiveRequests.ID).Value),
            MaximumHistoryEntriesPerEndpoint = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.MaximumHistoryEntriesPerEndpoint.ID).Value),
            HeaderText = settings.First(_ => _.ID == ServerSettingEntitiesConstants.HeaderText.ID).Value,
            MinimumSecondsBetweenFailureNotifications = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.MinimumSecondsBetweenFailureNotifications.ID).Value),
            UiRoutePath = settings.First(_ => _.ID == ServerSettingEntitiesConstants.UiRoutePath.ID).Value,
            NotifyUnHealthyOneTimeUntilChange = bool.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.NotifyUnHealthyOneTimeUntilChange.ID).Value)
        };
    
    public static ServerSettings MapToServerSettingsRequest(this IEnumerable<ServerSettingEntity> settings) =>
        new()
        {
            EvaluationTimeInSeconds = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.EvaluationTimeInSeconds.ID).Value),
            ApiMaxActiveRequests = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.ApiMaxActiveRequests.ID).Value),
            MaximumHistoryEntriesPerEndpoint = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.MaximumHistoryEntriesPerEndpoint.ID).Value),
            HeaderText = settings.First(_ => _.ID == ServerSettingEntitiesConstants.HeaderText.ID).Value,
            MinimumSecondsBetweenFailureNotifications = int.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.MinimumSecondsBetweenFailureNotifications.ID).Value),
            UiRoutePath = settings.First(_ => _.ID == ServerSettingEntitiesConstants.UiRoutePath.ID).Value,
            NotifyUnHealthyOneTimeUntilChange = bool.Parse(settings.First(_ => _.ID == ServerSettingEntitiesConstants.NotifyUnHealthyOneTimeUntilChange.ID).Value)
        };

    public static List<ServerSettingEntity> MapToServerSettingEntityList(this ServerSettings settings)
    {
        var evaluationTimeInSeconds = ServerSettingEntitiesConstants.EvaluationTimeInSeconds;
        var apiMaxActiveRequests = ServerSettingEntitiesConstants.ApiMaxActiveRequests;
        var maximumHistoryEntriesPerEndpoint = ServerSettingEntitiesConstants.MaximumHistoryEntriesPerEndpoint;
        var headerText = ServerSettingEntitiesConstants.HeaderText;
        var minimumSecondsBetweenFailureNotifications = ServerSettingEntitiesConstants.MinimumSecondsBetweenFailureNotifications;
        var uiRoutePath = ServerSettingEntitiesConstants.UiRoutePath;
        var notifyUnHealthyOneTimeUntilChange = ServerSettingEntitiesConstants.NotifyUnHealthyOneTimeUntilChange;
        
        evaluationTimeInSeconds.Value = settings.EvaluationTimeInSeconds.ToString();
        apiMaxActiveRequests.Value = settings.ApiMaxActiveRequests.ToString();
        maximumHistoryEntriesPerEndpoint.Value = settings.MaximumHistoryEntriesPerEndpoint.ToString();
        headerText.Value = settings.HeaderText;
        minimumSecondsBetweenFailureNotifications.Value = settings.MinimumSecondsBetweenFailureNotifications.ToString();
        uiRoutePath.Value = settings.UiRoutePath;
        notifyUnHealthyOneTimeUntilChange.Value = settings.NotifyUnHealthyOneTimeUntilChange.ToString();

        return new List<ServerSettingEntity>()
        {
            evaluationTimeInSeconds,
            apiMaxActiveRequests,
            maximumHistoryEntriesPerEndpoint,
            headerText,
            minimumSecondsBetweenFailureNotifications,
            uiRoutePath,
            notifyUnHealthyOneTimeUntilChange
        };
    }
}
