using HealthChecks.UI.Core;
using Microsoft.CodeAnalysis.Scripting;
using JollyChimp.Core.Common.Extensions;
using JollyChimp.Core.Services.Core.Contracts.Services;
using JollyChimp.Core.Services.Core.Models;

namespace JollyChimp.Core.Services.Services;

internal sealed class ExpressionValidationService : IExpressionValidationService
{
    
    private static ScriptOptions DefaultOptions => 
        CSharpScriptingExtensions
        .GetAssemblyReferenceOptions(typeof(UIHealthReport), typeof(DateTime))
        .AddImports("System");
    
    public async Task<PredicateParseResult> ValidateEndPointPredicateAsync(string predicate)
    {
        try
        {
            var parsedPredicate = await CSharpScriptingExtensions.ParseEndPointPredicateAsync(predicate, DefaultOptions);
            return PredicateParseResult.Success(predicate);
        }
        catch (Exception ex)
        {
            return PredicateParseResult.Failed(predicate, ex);
        }
    }
    
    public async Task<PredicateParseResult> ValidateWebHookPredicateAsync(string predicate)
    {
        try
        {
            var parsedPredicate = await CSharpScriptingExtensions.ParseWebHookPredicateAsync(predicate, DefaultOptions);
            return PredicateParseResult.Success(predicate);
        }
        catch (Exception ex)
        {
            return PredicateParseResult.Failed(predicate, ex);
        }
    }
    
}