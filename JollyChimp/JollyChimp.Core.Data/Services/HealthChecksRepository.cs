using System.Linq.Expressions;
using JollyChimp.Core.Common.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Data.Context;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Data.Extensions;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.Options;

namespace JollyChimp.Core.Data.Services;

internal sealed class HealthChecksRepository : IHealthChecksRepository
{

    private readonly JollyChimpContext _ctx;
    private readonly HealthCheckOptions _hcOptions;

    public HealthChecksRepository(JollyChimpContext ctx, IOptions<HealthCheckOptions> hcOptions)
    {
        _ctx = ctx;
        _hcOptions = hcOptions.Value;
    }

    public async Task<DataTableResponse<HealthCheckEntity>> GetHealthChecksAsync(DataTableRequest request)
    {
        var orderRequest = request.Order.FirstOrDefault() ?? new ()
        {
            Name = "name",
            Dir = "asc"
        };
        
        Expression<Func<HealthCheckEntity, object>> orderFunc = orderRequest.Name switch
        {
            "type" => unit => unit.Type,
            "tags" => unit => unit.Tags,
            "isEnabled" => unit => unit.IsEnabled,
            _ => unit => unit.Name //DEFAULT IS NAME
        };

        var filter = PredicateBuilder.New<HealthCheckEntity>(_ => !_.IsDeleted);

        if (request.Search != null && request.Search.Value != null)
        {
            var loweredSearchText = request.Search.Value.ToLower();
            
            var potentialTypes = _hcOptions.Types
                .Where(_ => _.ToString().ParseByCase().ToLower().Contains(loweredSearchText))
                .ToList();
            
            filter.And(
                x =>
                    x.Name.ToLower().Contains(loweredSearchText) ||
                    potentialTypes.Contains(x.Type) ||
                    x.Tags.ToLower().Contains(loweredSearchText)
            );
        }

        return await _ctx.HealthChecks
            .Include(_ => _.HealthCheckParameters)
            .Where(filter)
            .Sort(orderFunc, orderRequest.Dir == "asc")
            .PaginateDtResponseAsync(request);
    }

    public async Task<bool> CreateNewHealthCheck(HealthCheckEntity hcEntity)
    {
        await _ctx.HealthChecks.AddAsync(hcEntity);
        var addResult = await _ctx.SaveChangesAsync();
        return addResult > 0;
    }

    public async Task<bool> UpdateHealthCheckAsync(HealthCheckEntity hcEntity)
    {
        var targetCheck = await _ctx.HealthChecks
            .Include(_ => _.HealthCheckParameters)
            .Where(_ => _.ID == hcEntity.ID)
            .FirstOrDefaultAsync();
        
        if (targetCheck == null)
        {
            return false;
        }

        targetCheck.Name = hcEntity.Name;
        targetCheck.HealthStatus = hcEntity.HealthStatus;
        targetCheck.Tags = hcEntity.Tags;
        targetCheck.IsEnabled = hcEntity.IsEnabled;

        foreach (var param in hcEntity.HealthCheckParameters)
        {
            var targetParam = targetCheck.HealthCheckParameters.FirstOrDefault(p => p.Name == param.Name);
            if (targetParam == null)
            {
                targetCheck.HealthCheckParameters.Add(param);
            }
            else
            {
                targetParam.Value = param.Value;
            }
        }
        
        _ctx.HealthChecks.Update(targetCheck);
        var updateParamsResult = await _ctx.SaveChangesAsync();

        return updateParamsResult > 0;
    }

    public async Task<bool> DeleteHealthCheckAsync(int healthCheckId)
    {
        var checkResult = await _ctx.HealthChecks
            .Where(_ => _.ID == healthCheckId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsDeleted, true)
                );

        var parametersResult = await _ctx.HealthCheckParameters
            .Where(_ => _.HealthCheckId == healthCheckId)
            .ExecuteUpdateAsync(setters => setters
               .SetProperty(b => b.IsDeleted, true)
               );

        return checkResult > 0 && parametersResult > 0;
    }

    public Task<bool> HealthCheckNameExistsAsync(ValidateHealthCheckNameRequest request)
    {
        if (request.HealthCheckId.HasValue)
        {
            return _ctx.HealthChecks
                .AnyAsync(_ => _.Name == request.Name && _.ID != request.HealthCheckId.Value);
        }
       
        return _ctx.HealthChecks
            .AnyAsync(_ => _.Name == request.Name);
    }

}
