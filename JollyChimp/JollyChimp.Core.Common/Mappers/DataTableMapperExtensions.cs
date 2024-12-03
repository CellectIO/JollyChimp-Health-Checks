using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Common.Mappers;

public static class DataTableMapperExtensions
{
    public static DataTableResponse<TDestination> MapToNewDataTableResponse<TSource, TDestination>(
        this DataTableResponse<TSource> paginatedResponse, List<TDestination> updatedData)
    {
        return new DataTableResponse<TDestination>()
        {
            Draw = paginatedResponse.Draw,
            RecordsTotal = paginatedResponse.RecordsTotal,
            RecordsFiltered = paginatedResponse.RecordsFiltered,
            Data = updatedData
        };
    }
}