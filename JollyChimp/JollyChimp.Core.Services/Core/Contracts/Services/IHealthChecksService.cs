using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Base;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Services.Core.Contracts.Services;

public interface IHealthChecksService
{
    Task<DataTableResponse<HealthCheckApiResponse>> GetServerHealthChecksAsync(DataTableRequest request);
    Task<bool> UpsertHealthCheckAsync(UpsertHealthCheckRequest request);
    Task<bool> DeleteHealthCheckAsync(int healthCheckId);
    Task<bool> HealthCheckNameExistsAsync(ValidateHealthCheckNameRequest request);
}