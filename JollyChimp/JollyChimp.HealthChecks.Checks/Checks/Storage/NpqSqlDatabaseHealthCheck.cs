using HealthChecks.NpgSql;
using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Storage;

internal sealed class NpqSqlDatabaseHealthCheck : IJollyCheck
{
    
    public HealthCheckDefinition Definition => new()
    {
        Name = "NPQ SQL Database",
        Catergory = HealthCheckCategoryConstants.Storage,
        IsEnabled = true,
        Type = HealthCheckType.NpqSqlDatabase,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CONNECTION_STRING", "NPQ Database Connection String")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var connectionString = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING");
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        
        builder.AddNpgSql(
            options: new NpgSqlHealthCheckOptions(connectionString)
            {
                CommandText = "SELECT 1;"
            },
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}