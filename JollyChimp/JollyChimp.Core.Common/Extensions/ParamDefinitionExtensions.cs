using JollyChimp.Core.Common.Constants.UI;
using JollyChimp.Core.Common.Models.Base;
using JollyChimp.Core.Common.Models.Configs.Startup;

namespace JollyChimp.Core.Common.Extensions;

public static class ParamDefinitionExtensions
{
    private static ParameterDefinition Create(string name, string displayName, string formType)
        => new()
        {
            Name = name,
            DisplayName = displayName,
            FormType = formType
        };

    public static ParameterDefinition Text(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.Text);

    public static ParameterDefinition Url(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.Url);

    public static ParameterDefinition CheckBox(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.Checkbox);

    public static ParameterDefinition Number(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.Number);

    public static ParameterDefinition Password(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.Password);

    public static ParameterDefinition Tags(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.Tags);
    
    public static ParameterDefinition HeaderTags(string name, string displayName)
        => Create(name, displayName, ParameterFormTypeConstants.HeaderTags);

    public static ParameterDefinition WithRules(this ParameterDefinition def, params string[] rules)
    {
        def.ValidationRules.AddRange(rules);
        return def;
    }
    
    public static ParameterDefinition WithRequiredRule(this ParameterDefinition def)
    {
        return def.WithRules("required");
    }

    public static string ExtractRequiredParameter<TParam>(this List<TParam> hcParams, string name) where TParam : ParameterBase
    {
        return hcParams.First(p => p.Name == name).Value;
    }
    
    public static string? ExtractOptionalParameter<TParam>(this List<TParam> hcParams, string name) where TParam : ParameterBase
    {
        return hcParams.FirstOrDefault(p => p.Name == name)?.Value;
    }
    
}