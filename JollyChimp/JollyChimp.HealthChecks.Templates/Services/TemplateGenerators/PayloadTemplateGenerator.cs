using JollyChimp.HealthChecks.Templates.Services.Helpers;

namespace JollyChimp.HealthChecks.Templates.Services.TemplateGenerators;

public static class PayloadTemplateGenerator
{
    public static string Success()
    {
        return ContentRetriever.GetPayloadFile(true);
    }

    public static string Error()
    {
        return ContentRetriever.GetPayloadFile(false);
    }
}
