using JollyChimp.Core.Common.Models.Base;

namespace JollyChimp.Core.Common.Models.ApiRequests;

public class UpsertEndPointRequest : EndPointBase
{
    public int? ID { get; set; }
}
