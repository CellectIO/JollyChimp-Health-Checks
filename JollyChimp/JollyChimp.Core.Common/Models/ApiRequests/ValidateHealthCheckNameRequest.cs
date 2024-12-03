namespace JollyChimp.Core.Common.Models.ApiRequests;

public class ValidateHealthCheckNameRequest
{
    public int? HealthCheckId { get; set; }
    public string Name { get; set; }
}