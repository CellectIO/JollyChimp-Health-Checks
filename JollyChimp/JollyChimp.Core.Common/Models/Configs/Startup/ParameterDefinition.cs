namespace JollyChimp.Core.Common.Models.Configs.Startup;

public class ParameterDefinition
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string FormType { get; set; }
    public List<string> ValidationRules { get; set; }

    public ParameterDefinition()
    {
        ValidationRules = new();
    }
    
}