using System.Text;
using JollyChimp.HealthChecks.Templates.Core.Constants;
using JollyChimp.HealthChecks.Templates.Core.Models.Hooks.EmailHook;
using JollyChimp.HealthChecks.Templates.Services.Helpers;

namespace JollyChimp.HealthChecks.Templates.Services.TemplateGenerators;

public static class EmailHookTemplateGenerator
{

    public static string Error(Action<EmailFailures> configureConfig)
    {
        var model = new EmailFailures();
        configureConfig(model);

        ArgumentException.ThrowIfNullOrEmpty(model.FailureResource);
        ArgumentException.ThrowIfNullOrEmpty(model.FailureCount);
        ArgumentException.ThrowIfNullOrEmpty(model.ImageUrl);
        ArgumentException.ThrowIfNullOrEmpty(model.ViewFailuresUrl);
        ArgumentNullException.ThrowIfNull(model.Services);

        var baseTemplate = ContentRetriever.GetWebHooksFile("EmailHook", "down.html");
        var itemTemplate = ContentRetriever.GetWebHooksFile("EmailHook", "down-item.html");

        StringBuilder itemsHtml = new StringBuilder();
        foreach (var ser in model.Services)
        {
            var tempService = itemTemplate
               .Replace("{{SERVICE_NAME}}", ser.Key)
               .Replace("{{FAILURE_DURATION}}", ser.Value.Duration.ToString())
               .Replace("{{SERVICE_TAGS}}", string.Join(", ", ser.Value.Tags))
               .Replace("{{FAILURE_DESCRIPTION}}", ser.Value.Description);

            itemsHtml.AppendLine(tempService);
        }

        string content = baseTemplate
           .Replace("{{FAILURE_IMG_URL}}", model.ImageUrl)
           .Replace("{{FAILURE_RESOURCE}}", model.FailureResource)
           .Replace("{{FAILURE_COUNT}}", model.FailureCount)
           .Replace("{{FAILURE_SERVICE}}", itemsHtml.ToString())
           .Replace("{{VIEW_FAILURES_URL}}", model.ViewFailuresUrl);

        content = content.ReplaceThemeVars(model.Themes);

        return content;
    }

    public static string Success(Action<EmailRestored> configureConfig)
    {
        var model = new EmailRestored();
        configureConfig(model);

        ArgumentException.ThrowIfNullOrEmpty(model.RestoredResource);
        ArgumentException.ThrowIfNullOrEmpty(model.ActivityImg);
        ArgumentException.ThrowIfNullOrEmpty(model.ViewDashboardUrl);

        var content = ContentRetriever
            .GetWebHooksFile("EmailHook", "restored.html")
            .Replace("{{RESTORED_RESOURCE}}", model.RestoredResource)
            .Replace("{{CARD_ACTIVITY_IMG}}", model.ActivityImg)
            .Replace("{{VIEW_DASHBOARD_URL}}", model.ViewDashboardUrl);

        content = content.ReplaceThemeVars(model.Themes);

        return content;
    }

    private static string ReplaceThemeVars(this string template, ThemeConstants theme)
    {
        return template
            .Replace("{{THEME_MAIN_BG_COLOR}}", theme.MainBgColor)
            .Replace("{{THEME_CARD_BG_COLOR}}", theme.CardBgColor)
            .Replace("{{THEME_CARD_CONTENT_BG_COLOR}}", theme.CardContentBgColor)
            .Replace("{{THEME_TEXT_COLOR_HIGHLIGHTED}}", theme.TextColorHighlighted)
            .Replace("{{THEME_TEXT_COLOR_PRIMARY}}", theme.TextColorPrimary)
            .Replace("{{THEME_TEXT_COLOR_SECONDARY}}", theme.TextColorSecondary)
            .Replace("{{THEME_TEXT_COLOR_HIGHLIGHTED_SECONDARY}}", theme.TextColorHighlightedSecondary)
            .Replace("{{THEME_PRIMARY_TEXT_FONT_SIZE}}", theme.PrimaryTextFontSize)
            .Replace("{{THEME_PRIMARY_TEXT_LINE_HEIGHT}}", theme.PrimaryTextLineHeight)
            .Replace("{{THEME_PRIMARY_TEXT_FONT_WEIGHT}}", theme.PrimaryTextFontWeight)
            .Replace("{{THEME_SECONDARY_TEXT_FONT_SIZE}}", theme.SecondaryTextFontSize)
            .Replace("{{THEME_SECONDARY_TEXT_LINE_HEIGHT}}", theme.SecondaryTextLineHeight)
            .Replace("{{THEME_SECONDARY_TEXT_FONT_WEIGHT}}", theme.SecondaryTextFontWeight);
    }

}
