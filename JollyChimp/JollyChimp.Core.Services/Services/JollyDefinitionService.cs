using HealthChecks.UI.Configuration;
using JollyChimp.Core.Services.Core.Contracts.Services;
using JollyChimp.HealthChecks.Checks.Core.Models;
using JollyChimp.HealthChecks.WebHooks.Core.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace JollyChimp.Core.Services.Services;

internal sealed class JollyDefinitionService : IJollyDefinitionService
{

    private readonly IEnumerable<EndpointDataSource> _endpointSources;
    private readonly HealthCheckServiceOptions _healthChecks;
    private readonly WebHookOptions _webHookOptions;
    private readonly HealthCheckOptions _hcOptions;
    private readonly Settings _settings;

    public JollyDefinitionService(
        IEnumerable<EndpointDataSource> endpointSources, 
        IOptions<Settings> settings,
        IOptions<WebHookOptions> webHookOptions,
        IOptions<HealthCheckOptions> hcOptions,
        IOptions<HealthCheckServiceOptions> healthChecks)
    {
        _endpointSources = endpointSources;
        _healthChecks = healthChecks.Value;
        _settings = settings.Value;
        _webHookOptions = webHookOptions.Value;
        _hcOptions = hcOptions.Value;
    }
    
    public IList<HealthCheckDefinition> GetHealthCheckDefinitions()
    {
        var response = new List<HealthCheckDefinition>();
        
        var orderedCategories = _hcOptions.Definitions
            .Select(_ => _.Catergory)
            .DistinctBy(_ => _.Name)
            .OrderBy(_ => _.Order);
        
        foreach (var cat in orderedCategories)
        {
            var defs = _hcOptions.Definitions
                .Where(_ => _.Catergory.Name == cat.Name)
                .OrderBy(_ => _.Name)
                .ToList();
            
            response.AddRange(defs);
        }

        return response;
    }
    
    public IList<string> GetDeployedHealthCheckNames()
    {
        return _healthChecks.Registrations
            .Select(s => s.Name)
            .ToList();
    }

    public IList<WebHookDefinition> GetWebHooksDefinitions()
    {
        return _webHookOptions.Definitions;
    }
    
    public IList<string> GetDeployedWebHooks()
    {
        var propertyInfo = typeof(Settings).GetProperty("Webhooks", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        
        var value = (List<WebHookNotification>)propertyInfo.GetValue(_settings);
            
        return value.Select(_ => _.Name).ToList();
    }

    public List<string> GetDeployedEndpoints()
    {
        var endpoints = _endpointSources
            .SelectMany(es => es.Endpoints)
            .Where(e => e.DisplayName == "Health checks")
            .ToList();
        
        var routes = endpoints.Select(e =>
            {
                //TODO: THIS FEELS LIKE A MISTAKE TO GET THE ROUTE DICTIONARY ITEM USING AN INDEX
                var route = e.Metadata[1].ToString().Replace("Route: ", "");
                return route;
            }
        ).ToList();

        return routes;
    }

}
