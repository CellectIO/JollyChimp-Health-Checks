using JollyChimp.Core.Common.Models.Base;

namespace JollyChimp.Core.Common.Models.ApiResponses;

public class WebHookApiResponse : WebHookBase<ParameterApiResponse>
{
    public int ID { get; set; }
    public bool IsDeployed { get; set; }
}