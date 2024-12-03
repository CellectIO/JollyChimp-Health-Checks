using JollyChimp.Core.Common.Entities;

namespace JollyChimp.Core.Data.Core.Contracts.Services;

public interface IServerSettingsRepository
{
    Task<List<ServerSettingEntity>> GetSettingsAsync();
    Task<bool> UpsertSettingsAsync(List<ServerSettingEntity> settings);
}