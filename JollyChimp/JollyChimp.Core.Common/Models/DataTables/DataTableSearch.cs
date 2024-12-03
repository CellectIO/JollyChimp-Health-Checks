using System.Text.Json.Serialization;

namespace JollyChimp.Core.Common.Models.DataTables;

public class DataTableSearch
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }
    
    [JsonPropertyName("regex")]
    public bool UseRegex { get; set; }
}