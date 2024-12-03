using JollyChimp.Core.Common.Constants.Data;
using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Data.Context;
using JollyChimp.Core.Data.Core.Contracts.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JollyChimp.Core.Data.Services;

internal sealed class XabarilsRepository : IXabarilsRepository
{
    private readonly JollyChimpContext _ctx;

    public XabarilsRepository(JollyChimpContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> MarkEndPointAsDeletedAsync(EndPointEntity endpoint)
    {
        var toBeDeletedQueue = new List<DeleteQueueEntity>();

        var xabarilEndPoint = await _ctx.Configurations.FirstOrDefaultAsync(_ =>
            _.Name == endpoint.Name &&
            _.Uri == endpoint.ApiPath);
        
        var endPointFailures = await _ctx.Failures
            .Where(_ => _.HealthCheckName == endpoint.Name)
            .ToListAsync();

        var executions = await _ctx.Executions
            .Where(_ => _.Name == endpoint.Name &&
                        _.Uri == endpoint.ApiPath)
            .ToListAsync();
        
        if (xabarilEndPoint != null)
        {
            toBeDeletedQueue.Add(DeleteQueueEntity.CreateQueueEntity(SqlTableConstants.Configurations, xabarilEndPoint.Id));
        }
        
        if (endPointFailures.Any())
        {
            toBeDeletedQueue.AddRange(endPointFailures.Select(failure => DeleteQueueEntity.CreateQueueEntity(SqlTableConstants.Failures, failure.Id)));
        }
        
        foreach (var execution in executions)
        {
            toBeDeletedQueue.Add(DeleteQueueEntity.CreateQueueEntity(SqlTableConstants.Executions, execution.Id));

            var healthCheckExecutionHistories = _ctx.Database
                .SqlQueryRaw<int>(
                    @"select Id
                      from [JollyChimp].[HealthCheckExecutionHistories]
                      where HealthCheckExecutionId = @id", new SqlParameter("id", execution.Id))
                .ToList();

            var healthCheckExecutionEntries = _ctx.Database
                .SqlQueryRaw<int>(
                    @"select Id
                      from [JollyChimp].[HealthCheckExecutionEntries]
                      where HealthCheckExecutionId = @id", new SqlParameter("id", execution.Id))
                .ToList();
            
            toBeDeletedQueue.AddRange(healthCheckExecutionHistories.Select(ee => DeleteQueueEntity
                .CreateQueueEntity(SqlTableConstants.HealthCheckExecutionEntries, ee)));
            
            toBeDeletedQueue.AddRange(healthCheckExecutionEntries.Select(ee => DeleteQueueEntity
                .CreateQueueEntity(SqlTableConstants.HealthCheckExecutionEntries, ee)));
            
        }

        if (!toBeDeletedQueue.Any()) 
            return true;
        
        await _ctx.DeleteQueue.AddRangeAsync(toBeDeletedQueue);
        var insertQueueResult = await _ctx.SaveChangesAsync();
        return insertQueueResult > 0;
    }
    
}