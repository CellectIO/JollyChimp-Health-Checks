using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Services.Core.Contracts.Services;

namespace JollyChimp.Core.Services.Services;

internal sealed class HealthChecksService : IHealthChecksService
{
    private readonly IHealthChecksRepository _healthChecksRepository;
    private readonly IJollyDefinitionService _jollyDefinitionService;

    public HealthChecksService(
        IJollyDefinitionService jollyDefinitionService, 
        IHealthChecksRepository healthChecksRepository)
    {
        _jollyDefinitionService = jollyDefinitionService;
        _healthChecksRepository = healthChecksRepository;
    }
    
    public async Task<DataTableResponse<HealthCheckApiResponse>> GetServerHealthChecksAsync(DataTableRequest request)
    {
        var registeredChecks = _jollyDefinitionService.GetDeployedHealthCheckNames();
        var checks = await _healthChecksRepository.GetHealthChecksAsync(request);
       
        var response = checks.Data
            .Select(_ => _.MapToHealthCheckApiResponse(registeredChecks.Contains(_.Name)))
            .ToList();

        return checks.MapToNewDataTableResponse(response);
    }

    public async Task<bool> UpsertHealthCheckAsync(UpsertHealthCheckRequest request)
    {
        return request.ID.HasValue
            ? await _healthChecksRepository.UpdateHealthCheckAsync(request.MapToExistingHealthCheckEntity())
            : await _healthChecksRepository.CreateNewHealthCheck(request.MapToNewHealthCheckEntity());
    }

    public Task<bool> DeleteHealthCheckAsync(int healthCheckId)
        => _healthChecksRepository.DeleteHealthCheckAsync(healthCheckId);
    
    public Task<bool> HealthCheckNameExistsAsync(ValidateHealthCheckNameRequest request)
        => _healthChecksRepository.HealthCheckNameExistsAsync(request);

}