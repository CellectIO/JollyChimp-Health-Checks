using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Storage;

internal sealed class SqlServerDatabaseQueryHealthCheck : IJollyCheck
{
    
    public HealthCheckDefinition Definition => new()
    {
        Name = "Sql Server Database Query",
        Catergory = HealthCheckCategoryConstants.Storage,
        IsEnabled = true,
        Type = HealthCheckType.SqlServerDatabaseQuery,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CONNECTION_STRING", "Sql Server Connection String")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Text("SQL_QUERY", "Sql Server database Query")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var sqlConnectionString = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING");
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        var sqlQuery = config.Parameters.ExtractRequiredParameter("SQL_QUERY");
        
        builder.AddSqlServer(
            connectionString: sqlConnectionString,
            healthQuery: sqlQuery,
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}