using System.Text;
using JollyChimp.HealthChecks.Templates.Core.Models.Hooks.MicrosoftTeamsChannelHook;
using JollyChimp.HealthChecks.Templates.Services.Helpers;

namespace JollyChimp.HealthChecks.Templates.Services.TemplateGenerators;

public static class MicrosoftTeamsChannelHookTemplateGenerator
{

    public static string Error(Action<MicrosoftTeamsChannelFailures> configureConfig)
    {
        var model = new MicrosoftTeamsChannelFailures();
        configureConfig(model);

        ArgumentException.ThrowIfNullOrEmpty(model.FailureResource);
        ArgumentException.ThrowIfNullOrEmpty(model.FailureCount);
        ArgumentException.ThrowIfNullOrEmpty(model.ActivityImage);
        ArgumentException.ThrowIfNullOrEmpty(model.ViewFailuresUrl);
        ArgumentNullException.ThrowIfNull(model.Services);

        var sb = new StringBuilder();
        foreach (var service in model.Services)
        {
            sb.AppendLine($"<b>Name:</b> {service.Key}");
            sb.AppendLine($"<br>");
            sb.AppendLine($"<b>Duration:</b> {service.Value.Duration.ToString()}");

            if (service.Value.Tags != null && service.Value.Tags.Any())
            {
                sb.AppendLine($"<br>");
                sb.AppendLine($"<b>Tags:</b> {string.Join(", ", service.Value.Tags)}");
            }

            if (!string.IsNullOrWhiteSpace(service.Value.Description))
            {
                sb.AppendLine($"<br>");
                sb.AppendLine($"<b>Description:</b> {service.Value.Description}");
            }

            sb.AppendLine("\n---\n");
        }
        var failures = sb.ToString();

        var content = ContentRetriever
            .GetWebHooksFile("MicrosoftTeamsChannelHook", "down.json")
            .Replace("{{FAILURE_RESOURCE}}", model.FailureResource)
            .Replace("{{FAILURE_COUNT}}", model.FailureCount)
            .Replace("{{CARD_ACTIVITY_IMG}}", model.ActivityImage)
            .Replace("{{FAILURES}}", failures)
            .Replace("{{VIEW_FAILURES_URL}}", model.ViewFailuresUrl);

        return content;
    }

    public static string Success(Action<MicrosoftTeamsChannelRestored> configureConfig)
    {
        var model = new MicrosoftTeamsChannelRestored();
        configureConfig(model);

        ArgumentException.ThrowIfNullOrEmpty(model.RestoredResource);
        ArgumentException.ThrowIfNullOrEmpty(model.ActivityImg);
        ArgumentException.ThrowIfNullOrEmpty(model.ViewDashboardUrl);

        var content = ContentRetriever
            .GetWebHooksFile("MicrosoftTeamsChannelHook", "restored.json")
            .Replace("{{RESTORED_RESOURCE}}", model.RestoredResource)
            .Replace("{{CARD_ACTIVITY_IMG}}", model.ActivityImg)
            .Replace("{{VIEW_DASHBOARD_URL}}", model.ViewDashboardUrl);

        return content;
    }

}
