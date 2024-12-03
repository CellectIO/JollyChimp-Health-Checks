namespace JollyChimp.Core.Common.Models.Base;

public abstract class EndPointBase
{
    public string Name { get; set; }
    public string ApiPath { get; set; }
    public string HealthChecksPredicate { get; set; }
    public bool IsEnabled { get; set; }
}