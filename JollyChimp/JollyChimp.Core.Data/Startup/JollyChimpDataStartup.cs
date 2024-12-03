using HealthChecks.UI.Data;
using JollyChimp.Core.Data.Context;
using JollyChimp.Core.Data.Core.Contracts.Services;
using JollyChimp.Core.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace JollyChimp.Core.Data.Startup;

public static class JollyChimpDataStartup
{
    public static WebApplicationBuilder RegisterJollyChimpData(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddDataServices();

        return webApplicationBuilder;
    }

    private static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<IServerSettingsRepository, ServerSettingsRepository>();
        services.AddTransient<IEndPointsRepository, EndPointsRepository>();
        services.AddTransient<IWebHooksRepository, WebHooksRepository>();
        services.AddTransient<IHealthChecksRepository, HealthChecksRepository>();
        services.AddTransient<IXabarilsRepository, XabarilsRepository>();

        return services;
    }

    public static HealthChecksUIBuilder AddJollyChimpSqlServerStorage(
        this HealthChecksUIBuilder builder,
        string connectionString,
        Action<DbContextOptionsBuilder>? configureOptions = null,
        Action<SqlServerDbContextOptionsBuilder>? configureSqlServerOptions = null)
    {
        // NOTE:
        // Because internal services of Xabarils still relies on "HealthChecksDb", we will need to make sure it's still available.
        // SO if Something requires it, we will inject our version of the context (since it inherits from it already)
        builder.Services.AddScoped<HealthChecksDb, JollyChimpContext>();
        
        builder.Services.AddDbContext<JollyChimpContext>(optionsBuilder =>
        {
            configureOptions?.Invoke(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                sqlServerOptionsBuilder.MigrationsAssembly("JollyChimp.Core.Data");
                configureSqlServerOptions?.Invoke(sqlServerOptionsBuilder);
            });
        });

        return builder;
    }
}
