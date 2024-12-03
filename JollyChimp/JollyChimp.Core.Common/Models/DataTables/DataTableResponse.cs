namespace JollyChimp.Core.Common.Models.DataTables;

public class DataTableResponse<T>
{
    public int Draw { get; set; }

    public int RecordsTotal { get; set; }

    public int RecordsFiltered { get; set; }
    
    public List<T> Data { get; set; }

    public string? Error { get; set; }
    
    public static DataTableResponse<TSource> Success<TSource>(int draw, int recordsTotal, int recordsFiltered, List<TSource> data)
    {
        return new()
        {
            Draw = draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = data,
            Error = null
        };
    }
    
    public static DataTableResponse<TSource> Failure<TSource>(int draw, int recordsTotal, int recordsFiltered, string error)
    {
        return new()
        {
            Draw = draw,
            RecordsTotal = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data = new(),
            Error = error
        };
    }
    
}