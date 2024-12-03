using System.Linq.Expressions;
using JollyChimp.Core.Common.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Data.Context;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Data.Extensions;
using JollyChimp.HealthChecks.WebHooks.Core.Models;
using Microsoft.Extensions.Options;

namespace JollyChimp.Core.Data.Services;

internal sealed class WebHooksRepository : IWebHooksRepository
{

    private readonly JollyChimpContext _ctx;
    private readonly WebHookOptions _webHookOptions;

    public WebHooksRepository(JollyChimpContext ctx, IOptions<WebHookOptions> webHookOptions)
    {
        _ctx = ctx;
        _webHookOptions = webHookOptions.Value;
    }

    public async Task<DataTableResponse<WebHookEntity>> GetWebHooksAsync(DataTableRequest request)
    {
        var orderRequest = request.Order.FirstOrDefault() ?? new ()
        {
            Name = "name",
            Dir = "asc"
        };
        
        Expression<Func<WebHookEntity, object>> orderFunc = orderRequest.Name switch
        {
            "type" => unit => unit.Type,
            "isEnabled" => unit => unit.IsEnabled,
            _ => unit => unit.Name //DEFAULT IS NAME
        };

        var filter = PredicateBuilder.New<WebHookEntity>(_ => !_.IsDeleted);

        if (request.Search != null && request.Search.Value != null)
        {
            var loweredSearchText = request.Search.Value.ToLower();
            var potentialTypes = _webHookOptions.Types
                .Where(_ => _.ToString().ParseByCase().ToLower().Contains(loweredSearchText))
                .ToList();
            
            filter.And(
                x =>
                    x.Name.ToLower().Contains(loweredSearchText) ||
                    potentialTypes.Contains(x.Type)
            );
        }

        return await _ctx.WebHooks
            .Include(_ => _.WebHookParameters)
            .Where(filter)
            .Sort(orderFunc, orderRequest.Dir == "asc")
            .PaginateDtResponseAsync(request);
    }

    public async Task<bool> CreateNewWebHookAsync(WebHookEntity hookEntity)
    {
        await _ctx.WebHooks.AddAsync(hookEntity);
        var addResult = await _ctx.SaveChangesAsync();
        return addResult > 0;
    }

    public async Task<bool> UpdateWebHookAsync(WebHookEntity hookEntity)
    {
        var targetHook = await _ctx.WebHooks.FirstOrDefaultAsync(_ => _.ID == hookEntity.ID);
        if (targetHook == null)
        {
            return false;
        }

        //UPDATE HOOK
        var updateHookResult = await _ctx.WebHooks
            .Where(_ => _.ID == hookEntity.ID)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.Name, hookEntity.Name)
                .SetProperty(b => b.IsEnabled, hookEntity.IsEnabled)
                );

        //UPDATE HOOK PARAMETERS
        var parameterNames = hookEntity.WebHookParameters
            .Select(p => p.Name)
            .ToList();

        var savedParams = await _ctx.WebHookParameters
            .Where(p => parameterNames.Contains(p.Name))
            .ToListAsync();
            
        var updatedHookParameters = savedParams.Join(
            hookEntity.WebHookParameters,
            savedHook => savedHook.Name,
            updatedHook => updatedHook.Name,
            (savedHook, updatedHook) =>
            {
                savedHook.Value = updatedHook.Value;
                return savedHook;
            });
        _ctx.WebHookParameters.UpdateRange(updatedHookParameters);
        var updateParamsResult = await _ctx.SaveChangesAsync();

        return updateHookResult > 0 && updateParamsResult > 0;
    }

    public async Task<bool> DeleteWebHookAsync(int webHookId)
    {
        var hookResult = await _ctx.WebHooks
            .Where(_ => _.ID == webHookId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsDeleted, true)
                );

        var parametersResult = await _ctx.WebHookParameters
            .Where(_ => _.WebHookId == webHookId)
            .ExecuteUpdateAsync(setters => setters
               .SetProperty(b => b.IsDeleted, true)
               );

        return hookResult > 0 && parametersResult > 0;
    }

}
