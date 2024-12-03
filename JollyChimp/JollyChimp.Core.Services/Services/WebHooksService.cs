using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.DataTables;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Services.Core.Contracts.Services;

namespace JollyChimp.Core.Services.Services;

internal sealed class WebHooksService : IWebHooksService
{
    private readonly IWebHooksRepository _webHooksRepository;
    private readonly IJollyDefinitionService _jollyDefinitionService;

    public WebHooksService(IWebHooksRepository webHooksRepository, IJollyDefinitionService jollyDefinitionService)
    {
        _webHooksRepository = webHooksRepository;
        _jollyDefinitionService = jollyDefinitionService;
    }
    
    public async Task<DataTableResponse<WebHookApiResponse>> GetServerWebHooksAsync(DataTableRequest request)
    {
        var webHooks = await _webHooksRepository.GetWebHooksAsync(request);
        var runningHooks = _jollyDefinitionService.GetDeployedWebHooks();

        var response = webHooks.Data
            .Select(_ => _.MapToWebHookApiResponse(runningHooks.Contains(_.Name)))
            .ToList();

        return webHooks.MapToNewDataTableResponse(response);
    }

    public async Task<bool> UpsertWebHookAsync(UpsertWebHookRequest request)
    {
        return request.ID.HasValue
            ? await _webHooksRepository.UpdateWebHookAsync(request.MapToExistingWebHookEntity())
            : await _webHooksRepository.CreateNewWebHookAsync(request.MapToNewWebHookEntity());
    }

    public Task<bool> DeleteWebHookAsync(int webHookId)
        => _webHooksRepository.DeleteWebHookAsync(webHookId);

}