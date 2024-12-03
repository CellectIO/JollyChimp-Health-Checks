using JollyChimp.Core.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace JollyChimp.Core.Common.Seeds;

public static class ServerSettingEntitiesConstants
{

    public static ModelBuilder SeedServerSettings(this ModelBuilder builder)
    {
        builder.Entity<ServerSettingEntity>()
            .HasData(
                EvaluationTimeInSeconds,
                ApiMaxActiveRequests,
                MaximumHistoryEntriesPerEndpoint,
                HeaderText,
                MinimumSecondsBetweenFailureNotifications,
                NotifyUnHealthyOneTimeUntilChange,
                UiRoutePath
            );

        return builder;
    }

    public static ServerSettingEntity EvaluationTimeInSeconds => new()
    {
        ID = 1,
        Name = "EvaluationTimeInSeconds",
        Value = "10",
        IsDeleted = false,
        IsEnabled = true
    };

    public static ServerSettingEntity ApiMaxActiveRequests => new()
    {
        ID = 2,
        Name = "ApiMaxActiveRequests",
        Value = "50",
        IsDeleted = false,
        IsEnabled = true
    };

    public static ServerSettingEntity MaximumHistoryEntriesPerEndpoint => new()
    {
        ID = 3,
        Name = "MaximumHistoryEntriesPerEndpoint",
        Value = "50",
        IsDeleted = false,
        IsEnabled = true
    };

    public static ServerSettingEntity HeaderText => new()
    {
        ID = 4,
        Name = "HeaderText",
        Value = "Health Checks Dashboard",
        IsDeleted = false,
        IsEnabled = true
    };

    public static ServerSettingEntity MinimumSecondsBetweenFailureNotifications => new()
    {
        ID = 5,
        Name = "MinimumSecondsBetweenFailureNotifications",
        Value = "30",
        IsDeleted = false,
        IsEnabled = true
    };

    public static ServerSettingEntity NotifyUnHealthyOneTimeUntilChange => new()
    {
        ID = 6,
        Name = "NotifyUnHealthyOneTimeUntilChange",
        Value = "false",
        IsDeleted = false,
        IsEnabled = true
    };

    public static ServerSettingEntity UiRoutePath => new()
    {
        ID = 7,
        Name = "UiRoutePath",
        Value = "/hc-ui",
        IsDeleted = false,
        IsEnabled = true
    };
}
