using JollyChimp.Core.Common.Enums;

namespace JollyChimp.Core.Common.Models.Base;

public abstract class WebHookBase<TParamType>
{
    public string Name { get; set; }

    public WebHookTypes Type { get; set; }

    public bool IsEnabled { get; set; }

    public List<TParamType> Parameters { get; set; }
}