using JollyChimp.Core.Services.Core.Models;

namespace JollyChimp.Core.Services.Core.Contracts.Services;

public interface IExpressionValidationService
{
    Task<PredicateParseResult> ValidateEndPointPredicateAsync(string predicate);
    Task<PredicateParseResult> ValidateWebHookPredicateAsync(string predicate);
}