using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.ApiResponses;
using JollyChimp.Core.Common.Models.DataTables;

namespace JollyChimp.Core.Services.Core.Contracts.Services;

public interface IEndPointService
{
    Task<DataTableResponse<EndpointApiResponse>> GetServerEndPointsAsync(DataTableRequest request);
    Task<bool> EndPointApiPathExistsAsync(string apiPath);
    Task<bool> UpsertEndPointsAsync(UpsertEndPointRequest request);
    Task<bool> DeleteEndPointsAsync(int endPointId);
}