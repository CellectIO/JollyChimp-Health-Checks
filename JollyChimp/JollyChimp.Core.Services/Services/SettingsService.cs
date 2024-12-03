using JollyChimp.Core.Common.Mappers;
using JollyChimp.Core.Common.Models.ApiRequests;
using JollyChimp.Core.Common.Models.Base;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Services.Core.Contracts.Services;

namespace JollyChimp.Core.Services.Services;

internal sealed class SettingsService : ISettingsService
{
    private readonly IServerSettingsRepository _serverSettingsRepository;

    public SettingsService(IServerSettingsRepository serverSettingsRepository)
    {
        _serverSettingsRepository = serverSettingsRepository;
    }
    
    public async Task<ServerSettings> GetServerSettingsAsync()
    {
        var settings = await _serverSettingsRepository.GetSettingsAsync();
        return settings.MapToServerSettingsRequest();
    }

    public Task<bool> UpdateServerSettingsAsync(ServerSettings settings) => 
        _serverSettingsRepository.UpsertSettingsAsync(settings.MapToServerSettingEntityList());
}