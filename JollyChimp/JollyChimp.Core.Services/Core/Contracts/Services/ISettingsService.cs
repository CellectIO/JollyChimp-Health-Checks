using JollyChimp.Core.Common.Models.Base;

namespace JollyChimp.Core.Services.Core.Contracts.Services;

public interface ISettingsService
{
    Task<ServerSettings> GetServerSettingsAsync();
    Task<bool> UpdateServerSettingsAsync(ServerSettings settings);
}