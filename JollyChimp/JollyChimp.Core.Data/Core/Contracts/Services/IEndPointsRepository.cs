using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Data.Core.Contracts.Services;

public interface IEndPointsRepository
{
    Task<DataTableResponse<EndPointEntity>> GetEndPointsAsync(DataTableRequest request);
    Task<bool> CreateNewEndPointAsync(EndPointEntity entity);
    Task<bool> UpdateEndPointsAsync(EndPointEntity endpoint);
    Task<bool> DeleteEndPointsAsync(int endPointId);
    Task<bool> EndPointApiPathExistsAsync(string apiPath);
}