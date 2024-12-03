using System.Text.Json.Serialization;

namespace JollyChimp.Core.Common.Models.DataTables;

public class DataTableRequest
{

    [JsonPropertyName("draw")]
    public int Draw { get; set; }
    
    [JsonPropertyName("columns")]
    public List<DataTableColumn> Columns { get; set; } = new();
    
    [JsonPropertyName("order")]
    public List<DataTableOrder> Order { get; set; } = new();
    
    [JsonPropertyName("start")]
    public int Start { get; set; }
    
    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("search")]
    public DataTableSearch? Search { get; set; }
    
}