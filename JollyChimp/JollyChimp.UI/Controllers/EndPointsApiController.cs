using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Services.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace JollyChimp.UI.Controllers;

[ApiController]
[Route("api/endPoints")]
[Produces("application/json")]
public class EndPointsApiController : ControllerBase
{

    private readonly IExpressionValidationService _expressionValidationService;
    private readonly IEndPointService _endPointService;

    public EndPointsApiController(
        IExpressionValidationService expressionValidationService, 
        IEndPointService endPointService)
    {
        _expressionValidationService = expressionValidationService;
        _endPointService = endPointService;
    }

    [HttpPost]
    public async Task<IActionResult> GetServerEndPointsAsync([FromForm] DataTableRequest request)
    {
        var endPoints = await _endPointService.GetServerEndPointsAsync(request);
        return Ok(endPoints);
    }

    [HttpPost("validate/apipath")]
    public async Task<IActionResult> ValidateEndPointApiPath([FromBody] string apiPath)
    {
        var pathExists = await _endPointService.EndPointApiPathExistsAsync(apiPath);
        return Ok(!pathExists);
    }

    [HttpPost("validate/predicate")]
    public async Task<IActionResult> ValidateEndPointPredicateAsync([FromBody] string healthChecksPredicate)
    {
        var result = await _expressionValidationService.ValidateEndPointPredicateAsync(healthChecksPredicate);
        return Ok(result.IsSuccess);
    }

    [HttpPut]
    public async Task<IActionResult> UpsertServerEndPointAsync(UpsertEndPointRequest settings)
    {
        var result = await _endPointService.UpsertEndPointsAsync(settings);
        return result ? Ok() : BadRequest();
    }

    [HttpDelete("{endPointId:int}")]
    public async Task<IActionResult> DeleteServerEndPointAsync([FromRoute] int endPointId)
    {
        var result = await _endPointService.DeleteEndPointsAsync(endPointId);
        return result ? Ok() : BadRequest();
    }
}
