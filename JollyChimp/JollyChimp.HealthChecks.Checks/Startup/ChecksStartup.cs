using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.Checks.Core;
using JollyChimp.HealthChecks.Checks.Core.Contracts;
using JollyChimp.HealthChecks.Checks.Core.Models;

namespace JollyChimp.HealthChecks.Checks.Startup;

public static class ChecksStartup
{

    public static IHealthChecksBuilder RegisterJollyMonkeyChecks(this IHealthChecksBuilder hcBuilder,
        List<HealthCheckStartupConfig> healthCheckConfigs)
    {
        var checks = RetrieveChecksFromAssembly();
        
        hcBuilder.RegisterHealthCheckOptions(checks);
        hcBuilder.RegisterHealthChecks(healthCheckConfigs, checks);
        
        return hcBuilder;
    }

    private static List<IJollyCheck> RetrieveChecksFromAssembly()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Get all types in the assembly that implement IJollyCheck
        var types = assembly.GetTypes()
            .Where(t => 
                typeof(IJollyCheck).IsAssignableFrom(t) && 
                !t.IsInterface && !t.IsAbstract
                );

        // Create instances of each type
        List<IJollyCheck> instances = types
            .Select(type => (IJollyCheck)Activator.CreateInstance(type)!)
            .ToList();

        return instances;
    }

    private static IHealthChecksBuilder RegisterHealthCheckOptions(this IHealthChecksBuilder hcBuilder, List<IJollyCheck> checks)
    {
        var defs = checks.Select(c => c.Definition).ToList();
        var types = checks.Select(c => c.Definition.Type).ToList();

        hcBuilder.Services.Configure<HealthCheckOptions>(options =>
        {
            options.Definitions = defs;
            options.Types = types;
        });
        
        return hcBuilder;
    }
    
    private static IHealthChecksBuilder RegisterHealthChecks(
        this IHealthChecksBuilder hcBuilder, 
        List<HealthCheckStartupConfig> healthCheckConfigs,
        List<IJollyCheck> checks)
    {
        foreach (var check in healthCheckConfigs.Where(_ => _.IsEnabled))
        {
            var targetCheck = checks.First(c => c.Definition.Type == check.Type);
            hcBuilder = targetCheck.AddHealthCheck(hcBuilder, check);
        }
        
        return hcBuilder;
    }
    
}