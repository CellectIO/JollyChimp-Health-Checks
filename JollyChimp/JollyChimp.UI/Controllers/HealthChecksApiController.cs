using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Services.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace JollyChimp.UI.Controllers;

[ApiController]
[Route("api/healthchecks")]
[Produces("application/json")]
public class HealthChecksApiController : ControllerBase
{

    private readonly IHealthChecksService _healthChecksService;
    private readonly IJollyDefinitionService _jollyDefinitionService;
    
    public HealthChecksApiController(
        IHealthChecksService healthChecksService, 
        IJollyDefinitionService jollyDefinitionService)
    {
        _healthChecksService = healthChecksService;
        _jollyDefinitionService = jollyDefinitionService;
    }
    
    [HttpGet("definitions")]
    public IActionResult GetServerWebHookDefinitionsAsync()
    {
        var checks = _jollyDefinitionService.GetHealthCheckDefinitions();
        return Ok(checks);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetServerWebHooksAsync([FromForm] DataTableRequest request)
    {
        var response = await _healthChecksService.GetServerHealthChecksAsync(request);
        return Ok(response);
    }
    
    [HttpPost("validate/name")]
    public async Task<IActionResult> ValidateHealthCheckNameExistsAsync([FromBody] ValidateHealthCheckNameRequest request)
    {
        var nameExists = await _healthChecksService.HealthCheckNameExistsAsync(request);
        return Ok(!nameExists);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpsertServerWebHookAsync(UpsertHealthCheckRequest request)
    {
        var response = await _healthChecksService.UpsertHealthCheckAsync(request);
        return response ? Ok() : BadRequest();
    }
    
    [HttpDelete("{healthCheckId:int}")]
    public async Task<IActionResult> DeleteServerEndPointAsync([FromRoute] int healthCheckId)
    {
        var result = await _healthChecksService.DeleteHealthCheckAsync(healthCheckId);
        return result ? Ok() : BadRequest();
    }

}
