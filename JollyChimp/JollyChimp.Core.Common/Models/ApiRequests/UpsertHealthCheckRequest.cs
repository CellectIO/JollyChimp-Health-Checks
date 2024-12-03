using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Base;

namespace JollyChimp.Core.Common.Models.ApiRequests;

public class UpsertHealthCheckRequest : HealthCheckBase<ParameterApiResponse>
{
    public int? ID { get; set; }
}
