using JollyChimp.Core.Common.Entities;
using JollyChimp.Core.Data.Context;
using JollyChimp.Core.Data.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace JollyChimp.Core.Data.Services;

internal sealed class ServerSettingsRepository : IServerSettingsRepository
{

    private readonly JollyChimpContext _ctx;

    public ServerSettingsRepository(JollyChimpContext ctx)
    {
        _ctx = ctx;
    }

    public Task<List<ServerSettingEntity>> GetSettingsAsync() => 
        _ctx.ServerSettings
            .Where(_ => !_.IsDeleted)
            .ToListAsync();

    public async Task<bool> UpsertSettingsAsync(List<ServerSettingEntity> settings)
    {
        _ctx.ServerSettings.UpdateRange(settings);
        var updatedResult = await _ctx.SaveChangesAsync();
        return updatedResult > 0;
    }

}
