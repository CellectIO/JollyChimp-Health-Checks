using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Data.Core.Contracts.Services;

public interface IHealthChecksRepository
{
    Task<DataTableResponse<HealthCheckEntity>> GetHealthChecksAsync(DataTableRequest request);
    Task<bool> CreateNewHealthCheck(HealthCheckEntity hcEntity);
    Task<bool> UpdateHealthCheckAsync(HealthCheckEntity hcEntity);
    Task<bool> DeleteHealthCheckAsync(int healthCheckId);
    Task<bool> HealthCheckNameExistsAsync(ValidateHealthCheckNameRequest request);
}