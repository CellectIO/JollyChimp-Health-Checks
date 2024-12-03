using System.Text.Json.Serialization;

namespace JollyChimp.Core.Common.Models.DataTables;

public class DataTableOrder
{
    [JsonPropertyName("column")]
    public int Column { get; set; }
    
    [JsonPropertyName("dir")]
    public string Dir { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}