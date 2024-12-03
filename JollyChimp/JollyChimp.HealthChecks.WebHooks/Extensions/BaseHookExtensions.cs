using System.Text.Json;
using HealthChecks.UI.Configuration;
using HealthChecks.UI.Core;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.Core.Common.Models.HealthChecks;
using JollyChimp.HealthChecks.Templates.Services.TemplateGenerators;

namespace JollyChimp.HealthChecks.WebHooks.Extensions;

internal static class BaseHookExtensions
{
    public static Settings RegisterHook(
        this Settings uiSettings,
        WebHookStartupConfig webHookConfig,
        string endpoint
        )
    {
        if (!webHookConfig.IsEnabled)
        {
            return uiSettings;
        }

        var shouldExecuteFnc = webHookConfig.Parameters.First(p => p.Name == "SHOULD_NOTIFY_WHEN_EXPRESSION").Value;

        ArgumentException.ThrowIfNullOrWhiteSpace(shouldExecuteFnc);

        var shouldNotifyFunc = CSharpScriptingExtensions.ParseWebHookPredicate(
            shouldExecuteFnc,
            CSharpScriptingExtensions.GetAssemblyReferenceOptions(
                typeof(UIHealthReport),
                typeof(DateTime)
            ).AddImports(
                "System"
            ));

        uiSettings
            .AddWebhookNotification(webHookConfig.Name,
                uri: endpoint,
                payload: PayloadTemplateGenerator.Error(),
                restorePayload: PayloadTemplateGenerator.Success(),
                shouldNotifyFunc: shouldNotifyFunc,
                customMessageFunc: (livenessName, report) =>
                {
                    var checksReport = new WebHookHealthChecksReport(livenessName, report);
                    return JsonSerializer.Serialize(checksReport);
                },
                customDescriptionFunc: (livenessName, report) =>
                {
                    return report.Entries
                        .Count(e => e.Value.Status == UIHealthStatus.Unhealthy)
                        .ToString();
                });

        return uiSettings;
    }
}
