﻿using JollyChimp.Core.Common.Enums;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core.Constants;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using ParamDefinitionExtensions = JollyChimp.Core.Common.Extensions.ParamDefinitionExtensions;

namespace JollyChimp.HealthChecks.Checks.Checks.Storage;

internal sealed class SqlServerDatabaseHealthCheck : IJollyCheck
{
    
    public HealthCheckDefinition Definition => new()
    {
        Name = "Sql Server Database",
        Catergory = HealthCheckCategoryConstants.Storage,
        IsEnabled = true,
        Type = HealthCheckType.SqlServerDatabase,
        Parameters = new()
        {
            ParamDefinitionExtensions
                .Number("TIMEOUT", "Health Check timout (seconds)")
                .WithRequiredRule(),
            ParamDefinitionExtensions
                .Password("CONNECTION_STRING", "Sql Server Connection String")
                .WithRequiredRule()
        }
    };
    
    public IHealthChecksBuilder AddHealthCheck(IHealthChecksBuilder builder, HealthCheckStartupConfig config)
    {
        var sqlConnectionString = config.Parameters.ExtractRequiredParameter("CONNECTION_STRING");
        var timeout = config.Parameters.ExtractRequiredParameter("TIMEOUT");
        
        builder.AddSqlServer(
            connectionString: sqlConnectionString,
            healthQuery: "SELECT 1;",
            name: config.Name,
            failureStatus: config.FailureStatus,
            tags: config.Tags,
            timeout:  TimeSpan.FromSeconds(int.Parse(timeout))
        );

        return builder;
    }
}