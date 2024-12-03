using JollyChimp.Core.Common.Models.Base;

namespace JollyChimp.Core.Common.Models.ApiResponses;

public class HealthCheckApiResponse : HealthCheckBase<ParameterApiResponse>
{
    public int ID { get; set; }
    public bool IsDeployed { get; set; }
}