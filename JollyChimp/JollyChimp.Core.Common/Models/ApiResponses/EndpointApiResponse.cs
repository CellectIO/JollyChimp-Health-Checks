using JollyChimp.Core.Common.Models.Base;

namespace JollyChimp.Core.Common.Models.ApiResponses;

public class EndpointApiResponse : EndPointBase
{
    public int ID { get; set; }
    public bool IsDeployed { get; set; }
}
