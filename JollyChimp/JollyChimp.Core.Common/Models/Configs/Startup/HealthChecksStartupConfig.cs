namespace JollyChimp.Core.Common.Models.Configs.Startup;

public class HealthChecksStartupConfig
{

    public HealthChecksStartupConfig()
    {
        Server = new();
        UI = new();
        EndPoints = new();
        WebHooks = new();
        Checks = new();
    }

    public ServerStartupConfig Server { get; set; }

    public UIStartupConfig UI { get; set; }

    public List<EndPointStartupConfig> EndPoints { get; set; }

    public List<WebHookStartupConfig> WebHooks { get; set; }

    public List<HealthCheckStartupConfig> Checks { get; set; }
}