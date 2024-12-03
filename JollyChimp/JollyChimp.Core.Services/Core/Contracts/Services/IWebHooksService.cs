using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.Base;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Services.Core.Contracts.Services;

public interface IWebHooksService
{
    Task<DataTableResponse<WebHookApiResponse>> GetServerWebHooksAsync(DataTableRequest request);
    Task<bool> UpsertWebHookAsync(UpsertWebHookRequest request);
    Task<bool> DeleteWebHookAsync(int webHookId);
}