using JollyChimp.Core.Common.Entities;

namespace JollyChimp.Core.Data.Core.Contracts.Services;

public interface IXabarilsRepository
{
    Task<bool> MarkEndPointAsDeletedAsync(EndPointEntity endpoint);
}