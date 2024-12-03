using HealthChecks.UI.Core;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JollyChimp.Core.Common.Extensions;

public static class CSharpScriptingExtensions
{

    /// <summary>
    /// Creates required scripting options when trying to parse a string to a valid expressions.
    /// <br/>
    /// An example when trying to parse "Func HealthCheckRegistration" would mean we need to pass the type for HealthCheckRegistration. <br/>
    /// we do this by passing the following as a parameter: <br/>
    /// GetAssemblyReferenceOptions(typeof(HealthCheckRegistration))
    /// </summary>
    /// <param name="typeofClasses">contains the class types used in the epxression.</param>
    /// <returns></returns>
    public static ScriptOptions GetAssemblyReferenceOptions(params Type[] types)
    {
        //NEED TO ADD ASSEMBLY REFERENCE FOR CSHARP SCRIPTING TO WORK CORRECTLY.
        var options = ScriptOptions.Default
            .AddReferences(typeof(Func<,>).Assembly);

        foreach (var type in types)
        {
            options = options.AddReferences(type.Assembly);
        }

        return options;
    }

    public static ScriptOptions AddAssemblyImportOptions(this ScriptOptions options, params string[] imports)
    {
        foreach (var import in imports)
        {
            options = options.AddImports(import);
        }

        return options;
    }

    public static Func<string, UIHealthReport, bool> ParseWebHookPredicate(string predicate, ScriptOptions options)
    {
        return ParseWebHookPredicateAsync(predicate, options).Result;
    }

    public static Task<Func<string, UIHealthReport, bool>> ParseWebHookPredicateAsync(string predicate, ScriptOptions options)
    {
        return CSharpScript
           .EvaluateAsync<Func<string, UIHealthReport, bool>>(
               predicate,
               options
           );
    }

    public static Func<HealthCheckRegistration, bool> ParseEndPointPredicate(string predicate, ScriptOptions options)
    {
        return ParseEndPointPredicateAsync(predicate, options).Result;
    }

    public static Task<Func<HealthCheckRegistration, bool>> ParseEndPointPredicateAsync(string predicate, ScriptOptions options)
    {
        return CSharpScript
           .EvaluateAsync<Func<HealthCheckRegistration, bool>>(
               predicate,
               options
           );
    }

    public static T ParsePredicate<T>(string predicate, ScriptOptions options)
    {
        T func = CSharpScript
           .EvaluateAsync<T>(
               predicate,
               options
           ).Result;

        return func;
    }

}