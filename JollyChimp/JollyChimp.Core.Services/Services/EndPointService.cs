using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Services.Core.Contracts.Services;

namespace JollyChimp.Core.Services.Services;

internal sealed class EndPointService : IEndPointService
{
    
    private readonly IEndPointsRepository _endPointsRepository;
    private readonly IJollyDefinitionService _jollyDefinitionService;
    private readonly IExpressionValidationService _expressionValidationService;

    public EndPointService(
        IEndPointsRepository endPointsRepository, 
        IJollyDefinitionService jollyDefinitionService, 
        IExpressionValidationService expressionValidationService)
    {
        _endPointsRepository = endPointsRepository;
        _jollyDefinitionService = jollyDefinitionService;
        _expressionValidationService = expressionValidationService;
    }
    
    public async Task<DataTableResponse<EndpointApiResponse>> GetServerEndPointsAsync(DataTableRequest request)
    {
        var registeredEndPoints = _jollyDefinitionService.GetDeployedEndpoints();
        var endpoints = await _endPointsRepository.GetEndPointsAsync(request);

        var response = endpoints.Data
            .Select(_ => _.MapToEndpointApiResponse(registeredEndPoints.Contains(_.ApiPath)))
            .ToList();

        return endpoints.MapToNewDataTableResponse(response);
    }

    public Task<bool> EndPointApiPathExistsAsync(string apiPath)
        => _endPointsRepository.EndPointApiPathExistsAsync(apiPath);

    public async Task<bool> UpsertEndPointsAsync(UpsertEndPointRequest request)
    {
        var canParse = await _expressionValidationService.ValidateEndPointPredicateAsync(request.HealthChecksPredicate);
        if (!canParse.IsSuccess)
        {
            return canParse.IsSuccess;
        }
        
        return request.ID.HasValue
            ? await _endPointsRepository.UpdateEndPointsAsync(request.MapToExistingEndPointEntity())
            : await _endPointsRepository.CreateNewEndPointAsync(request.MapToNewEndPointEntity());
    }

    public Task<bool> DeleteEndPointsAsync(int endPointId)
        => _endPointsRepository.DeleteEndPointsAsync(endPointId);

}