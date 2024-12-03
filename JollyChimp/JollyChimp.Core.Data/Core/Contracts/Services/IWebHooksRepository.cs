using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Data.Core.Contracts.Services;

public interface IWebHooksRepository
{
    Task<DataTableResponse<WebHookEntity>> GetWebHooksAsync(DataTableRequest request);
    
    Task<bool> CreateNewWebHookAsync(WebHookEntity hookEntity);
    
    Task<bool> UpdateWebHookAsync(WebHookEntity hookEntity);
    
    Task<bool> DeleteWebHookAsync(int webHookId);
}