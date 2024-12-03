using JollyChimp.Core.Common.Models.Base;
using JollyChimp.Core.Services.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace JollyChimp.UI.Controllers;

[ApiController]
[Route("api/ServerSettings")]
[Produces("application/json")]
public class ServerSettingsApiController : ControllerBase
{

    private readonly ISettingsService _settingsService;

    public ServerSettingsApiController(
        ISettingsService settingsService
        )
    {
        _settingsService = settingsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetServerSettingsAsync()
    {
        var settings = await _settingsService.GetServerSettingsAsync();
        return Ok(settings);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateServerSettingsAsync(ServerSettings settings)
    {
        var result = await _settingsService.UpdateServerSettingsAsync(settings);
        return result ? Ok() : BadRequest();
    }

}