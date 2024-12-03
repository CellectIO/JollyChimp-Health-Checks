namespace JollyChimp.Core.Common.Models.Configs.Startup;

public class ServerStartupConfig
{
    /// <summary>
    /// Defines the current domain the application is hosted on.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// SQL Server Connection string used to store API results for history tracking
    /// </summary>
    public string SqlServerConnectionString { get; set; }

    public string EncryptionKey { get; set; }
}
