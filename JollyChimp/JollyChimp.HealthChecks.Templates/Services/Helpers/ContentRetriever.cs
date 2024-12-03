namespace JollyChimp.HealthChecks.Templates.Services.Helpers;

internal static class ContentRetriever
{

    public static string GetWebHooksFile(params string[] paths)
    {
        var path = string.Join("\\", paths);
        var fullPath = string.Format(
            "{0}{1}{2}", 
            AppDomain.CurrentDomain.BaseDirectory,
            "Content\\WebHooks\\Hooks\\",
            path);

        return Get(fullPath);
    }

    public static string GetPayloadFile(bool isRestoreFile)
    {
        var fileName = isRestoreFile ? "restorePayload.json" : "payload.json";
        var fullPath = string.Format(
            "{0}{1}{2}",
            AppDomain.CurrentDomain.BaseDirectory,
            "Content\\WebHooks\\Payload\\",
            fileName);

        return Get(fullPath);
    }

    private static string Get(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File does not exist: {filePath}");

        return File.ReadAllText(filePath);
    }
}
