using JollyChimp.Core.Common.Constants.Data;
using Microsoft.EntityFrameworkCore;
using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.Core.Data.Context;

namespace JollyChimp.Core.Data.Builders;

public class JollyChimpStartupConfigBuilder : IDisposable
{

    private HealthChecksStartupConfig Config { get; set; }

    private readonly JollyChimpContext _ctx;

    public JollyChimpStartupConfigBuilder(JollyChimpContext ctx, HealthChecksStartupConfig config)
    {
        _ctx = ctx;
        Config = config;
    }

    public static JollyChimpStartupConfigBuilder Init(ServerStartupConfig config)
    {
        var ctxOptions = new DbContextOptionsBuilder<JollyChimpContext>()
            .UseSqlServer(config.SqlServerConnectionString)
            .Options;

        var initConfig = new HealthChecksStartupConfig()
        {
            Server = config
        };

        var ctx = new JollyChimpContext(ctxOptions);
        
        ctx.Database.Migrate();

        return new(ctx, initConfig);
    }

    public JollyChimpStartupConfigBuilder ClearDeleteQueue()
    {
        var deleteQueue = this._ctx.DeleteQueue
            .Where(dq => !dq.IsDeleted)
            .ToList();

        var healthCheckExecutionHistories = deleteQueue
            .Where(dq => dq.XabarilTableName == SqlTableConstants.HealthCheckExecutionHistories)
            .Select(dq => dq.XabarilId)
            .ToList();
        var healthCheckExecutionHistoriesResult = this._ctx.HealthCheckExecutionHistories
            .Where(hceh => healthCheckExecutionHistories.Contains(hceh.Id))
            .ExecuteDelete();

        var healthCheckExecutionEntries = deleteQueue
            .Where(dq => dq.XabarilTableName == SqlTableConstants.HealthCheckExecutionEntries)
            .Select(dq => dq.XabarilId)
            .ToList();
        var healthCheckExecutionEntriesResult = this._ctx.HealthCheckExecutionEntries
            .Where(hceh => healthCheckExecutionEntries.Contains(hceh.Id))
            .ExecuteDelete();

        var executions = deleteQueue
            .Where(dq => dq.XabarilTableName == SqlTableConstants.Executions)
            .Select(dq => dq.XabarilId)
            .ToList();
        var executionsResult = this._ctx.Executions
            .Where(hceh => executions.Contains(hceh.Id))
            .ExecuteDelete();

        var configurations = deleteQueue
            .Where(dq => dq.XabarilTableName == SqlTableConstants.Configurations)
            .Select(dq => dq.XabarilId)
            .ToList();
        var configurationsResult = this._ctx.Configurations
            .Where(hceh => configurations.Contains(hceh.Id))
            .ExecuteDelete();

        var failures = deleteQueue
            .Where(dq => dq.XabarilTableName == SqlTableConstants.Failures)
            .Select(dq => dq.XabarilId)
            .ToList();
        var failuresResult = this._ctx.Failures
            .Where(hceh => failures.Contains(hceh.Id))
            .ExecuteDelete();

        var deleteQueueIds = deleteQueue
                .Select(q => q.ID)
                .ToList();
        var deleteQueueResult = this._ctx.DeleteQueue
            .Where(dq => deleteQueueIds.Contains(dq.ID)
            )
            .ExecuteUpdate(dq =>
                dq.SetProperty(p => p.IsDeleted, true)
            );
        
        return this;
    }
    
    public JollyChimpStartupConfigBuilder SetUiSettings()
    {
        Config.UI = _ctx.ServerSettings
            .Where(_ => !_.IsDeleted)
            .ToList()
            .MapToUIStartupConfig();

        return this;
    }

    public JollyChimpStartupConfigBuilder SetEndPoints()
    {
        Config.EndPoints = _ctx.EndPoints
            .Where(_ => !_.IsDeleted && _.IsEnabled)
            .ToList()
            .MapToEndPointStartupConfigList();

        return this;
    }

    public JollyChimpStartupConfigBuilder SetWebHooks()
    {
        Config.WebHooks = _ctx.WebHooks
            .Include(_ => _.WebHookParameters)
            .Where(_ => !_.IsDeleted)
            .ToList()
            .MapToWebHookStartupConfigList();

        return this;
    }

    public JollyChimpStartupConfigBuilder SetHealthChecks()
    {
        Config.Checks = _ctx.HealthChecks
            .Include(_ => _.HealthCheckParameters)
            .Where(_ => !_.IsDeleted)
            .MapToHealthCheckStartupConfigList();

        return this;
    }

    public HealthChecksStartupConfig Return()
    {
        return Config;
    }
    
    public void Dispose()
    {
        _ctx.Dispose();
    }
    
}
