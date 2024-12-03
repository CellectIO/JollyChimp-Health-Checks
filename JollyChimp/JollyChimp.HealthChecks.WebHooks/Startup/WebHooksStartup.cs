using System.Reflection;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Routing;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.HealthChecks.WebHooks.Core.Contracts;
using JollyChimp.HealthChecks.WebHooks.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JollyChimp.HealthChecks.WebHooks.Startup;

public static class WebHooksStartup
{
    
    public static IServiceCollection RegisterWebHookOptions(this IServiceCollection services)
    {
        var hooks = RetrieveHooksFromAssembly();
        
        var defs = hooks.Select(c => c.Definition).ToList();
        var types = hooks.Select(c => c.Definition.Type).ToList();

        services.Configure<WebHookOptions>(options =>
        {
            options.Definitions = defs;
            options.Types = types;
        });
        
        return services;
    }

    public static Settings RegisterJollyChimpWebHooks(
        this Settings uiSettings, 
        HealthChecksStartupConfig config)
    {
        var hooks = RetrieveHooksFromAssembly();
        uiSettings.RegisterWebHooks(config, hooks);

        return uiSettings;
    }

    public static IEndpointRouteBuilder UseJollyChimpWebHooks(this IEndpointRouteBuilder endpointRouteBuilder, HealthChecksStartupConfig config)
    {
        var hooks = RetrieveHooksFromAssembly();
        endpointRouteBuilder.UseWebHooks(config, hooks);

        return endpointRouteBuilder;
    }

    private static List<IJollyHook> RetrieveHooksFromAssembly()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Get all types in the assembly that implement IJollyHook
        var types = assembly.GetTypes()
            .Where(t => 
                typeof(IJollyHook).IsAssignableFrom(t) && 
                !t.IsInterface && !t.IsAbstract
            );

        // Create instances of each type
        List<IJollyHook> instances = types
            .Select(type => (IJollyHook)Activator.CreateInstance(type)!)
            .ToList();

        return instances;
    }
    
    private static Settings RegisterWebHooks(
        this Settings uiSettings, 
        HealthChecksStartupConfig config,
        List<IJollyHook> hooks)
    {
        foreach (var hook in config.WebHooks.Where(_ => _.IsEnabled))
        {
            var targetCheck = hooks.First(c => c.Definition.Type == hook.Type);
            uiSettings = targetCheck.RegisterHook(uiSettings, hook, config);
        }
        
        return uiSettings;
    }
    
    private static IEndpointRouteBuilder UseWebHooks(
        this IEndpointRouteBuilder endpointRouteBuilder, 
        HealthChecksStartupConfig config,
        List<IJollyHook> hooks)
    {
        foreach (var hook in config.WebHooks.Where(_ => _.IsEnabled))
        {
            var targetCheck = hooks.First(c => c.Definition.Type == hook.Type);
            endpointRouteBuilder = targetCheck.UseHook(endpointRouteBuilder, hook, config);
        }
        
        return endpointRouteBuilder;
    }
    
}