namespace JollyChimp.Core.Common.Models.Base;

public class ServerSettings
{
    /// <summary>
    /// time in seconds between check
    /// </summary>
    public int EvaluationTimeInSeconds { get; set; }

    /// <summary>
    /// api requests concurrency
    /// </summary>
    public int ApiMaxActiveRequests { get; set; }

    /// <summary>
    /// maximum history of checks
    /// </summary>
    public int MaximumHistoryEntriesPerEndpoint { get; set; }

    public string HeaderText { get; set; }

    public int MinimumSecondsBetweenFailureNotifications { get; set; }

    public bool NotifyUnHealthyOneTimeUntilChange { get; set; }

    /// <summary>
    /// Defines the path to the health checks page.
    /// </summary>
    public string UiRoutePath { get; set; }
}