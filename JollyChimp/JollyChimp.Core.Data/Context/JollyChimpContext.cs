using HealthChecks.UI.Data;
using JollyChimp.Core.Common.Constants.Data;
using JollyChimp.Core.Common.Encryption;
using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Seeds;
using Microsoft.EntityFrameworkCore;

namespace JollyChimp.Core.Data.Context;

public class JollyChimpContext : HealthChecksDb
{

    #region JOLLY-CHIMP

    public DbSet<EndPointEntity> EndPoints { get; set; }
    public DbSet<WebHookEntity> WebHooks { get; set; }
    public DbSet<WebHookParameterEntity> WebHookParameters { get; set; }
    public DbSet<HealthCheckEntity> HealthChecks { get; set; }
    public DbSet<HealthCheckParameterEntity> HealthCheckParameters { get; set; }
    public DbSet<ServerSettingEntity> ServerSettings { get; set; }
    public DbSet<DeleteQueueEntity> DeleteQueue { get; set; }

    #endregion
    
    public JollyChimpContext(DbContextOptions<JollyChimpContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SqlSchemaConstants.JollyChimp);
        
        modelBuilder.UseEncryption();
        modelBuilder.SeedServerSettings();
        
        base.OnModelCreating(modelBuilder);
    }

}


