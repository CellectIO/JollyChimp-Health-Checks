using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Data.Extensions;

internal static class QueryableExtensions
{
    public static async Task<DataTableResponse<TSource>> PaginateDtResponseAsync<TSource>(
        this IQueryable<TSource> queryable,
        DataTableRequest request)
    {
        var paginatedResult = await queryable
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();
        
        var totalRecords = await queryable.CountAsync();

        return new DataTableResponse<TSource>()
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords, //TODO: THIS VALUE IS MOST LIKELY THE TOTAL COUNT OF RECORDS IN THE TABLE FOR FILTERING?
            RecordsFiltered = totalRecords,
            Data = paginatedResult
        };
    }
    
    public static IOrderedQueryable<TSource> Sort<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, object>> orderBy,
        bool isAsc = true
    )
    {
        if (isAsc)
        {
            return source.OrderBy(orderBy);
        }
		
        return source.OrderByDescending(orderBy);
    }
}