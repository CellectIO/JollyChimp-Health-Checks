namespace JollyChimp.Core.Services.Core.Models;

public class PredicateParseResult
{
    public string Predicate { get; set; }
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }

    public static PredicateParseResult Success(string predicate) =>
        new()
        {
            Predicate = predicate,
            IsSuccess = true
        };
    
    public static PredicateParseResult Failed(string predicate, Exception exception) =>
        new()
        {
            Predicate = predicate,
            IsSuccess = false,
            Exception = exception
        };
}