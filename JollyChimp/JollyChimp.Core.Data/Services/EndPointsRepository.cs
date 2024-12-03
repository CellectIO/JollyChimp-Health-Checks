using System.Linq.Expressions;
using JollyChimp.Core.Common.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Data.Context;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Data.Extensions;

namespace JollyChimp.Core.Data.Services;

internal sealed class EndPointsRepository : IEndPointsRepository
{

    private readonly JollyChimpContext _ctx;
    private readonly IXabarilsRepository _xabarilsRepository;

    public EndPointsRepository(
        JollyChimpContext ctx, 
        IXabarilsRepository xabarilsRepository)
    {
        _ctx = ctx;
        _xabarilsRepository = xabarilsRepository;
    }

    public async Task<DataTableResponse<EndPointEntity>> GetEndPointsAsync(DataTableRequest request)
    {
        var orderRequest = request.Order.FirstOrDefault() ?? new ()
        {
            Name = "name",
            Dir = "asc"
        };
        
        Expression<Func<EndPointEntity, object>> orderFunc = orderRequest.Name switch
        {
            "healthChecksPredicate" => unit => unit.HealthChecksPredicate,
            "apiPath" => unit => unit.ApiPath,
            "isEnabled" => unit => unit.IsEnabled,
            _ => unit => unit.Name //DEFAULT IS NAME
        };

        var filter = PredicateBuilder.New<EndPointEntity>(_ => !_.IsDeleted);

        if (request.Search != null && request.Search.Value != null)
        {
            var loweredSearchText = request.Search.Value.ToLower();
            filter.And(
                x =>
                    x.Name.ToLower().Contains(loweredSearchText) ||
                    x.ApiPath.ToLower().Contains(loweredSearchText) ||
                    x.HealthChecksPredicate.ToLower().Contains(loweredSearchText)
                );
        }

        return await _ctx.EndPoints
            .Where(filter)
            .Sort(orderFunc, orderRequest.Dir == "asc")
            .PaginateDtResponseAsync(request);
    }

    public async Task<bool> CreateNewEndPointAsync(EndPointEntity entity)
    {
        await _ctx.EndPoints.AddAsync(entity);
        var updatedResult = await _ctx.SaveChangesAsync();
        return updatedResult > 0;
    }

    public async Task<bool> UpdateEndPointsAsync(EndPointEntity endpoint)
    {
        var targetEndpoint = await _ctx.EndPoints.FirstOrDefaultAsync(_ => _.ID == endpoint.ID);
        if (targetEndpoint == null)
        {
            return false;
        }
        
        // NOTE:
        // Since the Xabarils framework tracks endpoints based on the endpoint name and api,
        // and health checks based on the name and tags.
        // IF we modify those details on our end, it will causes issues with the internals of xabarils.
        // So as an temporary measure we delete all the tracking data.
        // TODO:
        // This is not the best, so as an enhancement, rather see if we can update the existing saved data
        var queueResult = await _xabarilsRepository.MarkEndPointAsDeletedAsync(targetEndpoint);
        
        var updateResult = await _ctx.EndPoints.Where(_ => _.ID == endpoint.ID)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.Name, endpoint.Name)
                .SetProperty(b => b.ApiPath, endpoint.ApiPath)
                .SetProperty(b => b.HealthChecksPredicate, endpoint.HealthChecksPredicate)
                .SetProperty(b => b.IsEnabled, endpoint.IsEnabled)
                );

        return updateResult > 0;
    }

    public async Task<bool> DeleteEndPointsAsync(int endPointId)
    {
        var targetJollyEndPoint = await _ctx.EndPoints
            .FirstAsync(_ => _.ID == endPointId);

        var queueResult = await _xabarilsRepository.MarkEndPointAsDeletedAsync(targetJollyEndPoint);

        var jollyDeleteResult = await _ctx.EndPoints
            .Where(_ => _.ID == endPointId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsDeleted, true)
                );

        return jollyDeleteResult > 0;
    }

    public Task<bool> EndPointApiPathExistsAsync(string apiPath) =>
        _ctx.EndPoints
            .Where(_ => _.ApiPath == apiPath && !_.IsDeleted)
            .AnyAsync();

}
