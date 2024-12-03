using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Services.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace JollyChimp.UI.Controllers;

[ApiController]
[Route("api/webhooks")]
[Produces("application/json")]
public class WebHooksApiController : ControllerBase
{

    private readonly IExpressionValidationService _expressionValidationService;
    private readonly IWebHooksService _webHooksService;
    private readonly IJollyDefinitionService _jollyDefinitionService;
    
    public WebHooksApiController(
        IExpressionValidationService expressionValidationService, 
        IWebHooksService webHooksService, 
        IJollyDefinitionService jollyDefinitionService)
    {
        _expressionValidationService = expressionValidationService;
        _webHooksService = webHooksService;
        _jollyDefinitionService = jollyDefinitionService;
    }
    
    [HttpGet("definitions")]
    public IActionResult GetServerWebHookDefinitionsAsync()
    {
        var response = _jollyDefinitionService.GetWebHooksDefinitions();
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetServerWebHooksAsync([FromForm] DataTableRequest request)
    {
        var endPoints = await _webHooksService.GetServerWebHooksAsync(request);
        return Ok(endPoints);
    }
    
    [HttpPost("validate/predicate")]
    public async Task<IActionResult> ValidateWebHookPredicateAsync([FromBody] string webHookPredicate)
    {
        var result = await _expressionValidationService.ValidateWebHookPredicateAsync(webHookPredicate);
        return Ok(result.IsSuccess);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpsertServerWebHookAsync(UpsertWebHookRequest request)
    {
        var response = await _webHooksService.UpsertWebHookAsync(request);
        return response ? Ok() : BadRequest();
    }
    
    [HttpDelete("{webHookId:int}")]
    public async Task<IActionResult> DeleteServerEndPointAsync([FromRoute] int webHookId)
    {
        var result = await _webHooksService.DeleteWebHookAsync(webHookId);
        return result ? Ok() : BadRequest();
    }

}