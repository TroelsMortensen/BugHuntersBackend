using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Common.Result;

public abstract record Result;

public abstract record Failure : Result;

public abstract record Result<T> : Result
{
    public static implicit operator Result<T>(ResultError error) =>
        Failure<T>(error);

    public static implicit operator Result<T>(T value) =>
        Success(value);
}

public record Success<T>(T Value) : Result<T>;

public record Failure<T>(IEnumerable<ResultError> Errors) : Result<T>;

public record ResultError(string Code, string Message);

public record None;

public static class ResultExt
{
    public static Result<T> Success<T>(T value) =>
        new Success<T>(value);

    public static Result<None> Success() =>
        None;

    public static Result<T> Failure<T>(params ResultError[] errors) =>
        new Failure<T>(errors);

    public static Result<None> Failure(params ResultError[] errors) =>
        new Failure<None>(errors);

    public static Result<T> ToResult<T>(this T value) =>
        value switch
        {
            null => Failure<T>(new ResultError("NullValue", "Value is null.")),
            _ => Success(value)
        };

    public static Result<R> Map<T, R>(this Result<T> result, Func<T, R> func) =>
        result switch
        {
            Success<T> successResult => Success(func(successResult.Value)),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static async Task<Result<R>> Map<T, R>(this Result<T> result, Func<T, Task<R>> func) =>
        result switch
        {
            Success<T> successResult => Success(await func(successResult.Value)),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static async Task<Result<R>> Map<T, R>(this Task<Result<T>> result, Func<T, Task<Result<R>>> func) =>
        (await result) switch
        {
            Success<T> successResult => await func(successResult.Value),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static async Task<Result<R>> Map<T, R>(this Task<Result<T>> result, Func<T, Result<R>> func) =>
        (await result) switch
        {
            Success<T> successResult => func(successResult.Value),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    /**
     * Bind function
     */
    public static Result<R> Bind<T, R>(this Result<T> result, Func<T, Result<R>> func) =>
        result switch
        {
            Success<T> successResult => func(successResult.Value),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static async Task<Result<R>> Bind<T, R>(this Result<T> result, Func<T, Task<Result<R>>> func) =>
        result switch
        {
            Success<T> successResult => await func(successResult.Value),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static async Task<Result<R>> Bind<T, R>(this Task<Result<T>> result, Func<T, Result<R>> func) =>
        (await result) switch
        {
            Success<T> successResult => func(successResult.Value),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };


    public static async Task<Result<R>> Bind<T, R>(this Task<Result<T>> result, Func<T, Task<Result<R>>> func) =>
        (await result) switch
        {
            Success<T> successResult => await func(successResult.Value),
            Failure<T> failureResult => Failure<R>(failureResult.Errors.ToArray()),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    /**
     * Tee function
     */
    public static async Task<Result<T>> Tee<T>(this Result<T> result, Func<Task<Result<None>>> func) =>
        result switch
        {
            Success<T> => await PerformTeeAsync(result, func),
            _ => result
        };

    public static async Task<Result<T>> Tee<T>(this Task<Result<T>> result, Func<Task<Result<None>>> func) =>
        (await result) switch
        {
            Success<T> success => await PerformTeeAsync(success, func),
            _ => await result
        };

    public static async Task<Result<T>> Tee<T>(this Task<Result<T>> result, Func<Result<None>> func) =>
        (await result) switch
        {
            Success<T> success => PerformTee(success, func),
            _ => await result
        };

    private static async Task<Result<T>> PerformTeeAsync<T>(Result<T> result, Func<Task<Result<None>>> func) =>
        (await func()).Match(
            _ => result,
            errors => Failure<T>(errors.ToArray())
        );

    private static Result<T> PerformTee<T>(Result<T> result, Func<Result<None>> func) =>
        func().Match(
            _ => result,
            errors => Failure<T>(errors.ToArray())
        );

    /**
     * Validation
     */
    // public static Result<None> StartValidation() =>
    //     Success();
    //
    // public static Result<None> AssertThat(this Result<None> result, Func<Result<None>> func) =>
    //     new List<Result<None>>
    //         {
    //             result, func()
    //         }
    //         .Merge();

    public static Result<None> AssertAll(params Func<Result<None>>[] validations) =>
        validations
            .Select(validation => validation())
            .Merge();


    public static Result<T> WithPayloadIfSuccess<T>(this Result<None> result, Func<T> payload) =>
        result switch
        {
            Success<T> => Success(payload()),
            Failure<T> failure => failure,
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static R Match<T, R>(this Result<T> result, Func<T, R> onSuccess, Func<IEnumerable<ResultError>, R> onFailure) =>
        result switch
        {
            Success<T> successResult => onSuccess(successResult.Value),
            Failure<T> failureResult => onFailure(failureResult.Errors),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static async Task<R> Match<T, R>(this Task<Result<T>> result, Func<T, R> onSuccess,
        Func<IEnumerable<ResultError>, R> onFailure) =>
        result switch
        {
            Success<T> successResult => onSuccess(successResult.Value),
            Failure<T> failureResult => onFailure(failureResult.Errors),
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static T ValueOr<T>(this Result<T> result, Func<IEnumerable<ResultError>, T> onFailure) =>
        result switch
        {
            Success<T> successResult => successResult.Value,
            Failure<T> failure => onFailure(failure.Errors),
            _ => throw new ArgumentException("Unknown type of result.")
        };


    public static Result<None> Combine(params Result[] results) =>
        results
            .ToList()
            .Merge();

    private static Result<None> Merge(this IEnumerable<Result<None>> all) =>
        all.SelectMany(result =>
                result switch
                {
                    Failure<None> failure => failure.Errors,
                    _ => Array.Empty<ResultError>()
                })
            .ErrorsToSingleResult();


    public static Result<TOut> ValuesToObject<T1, T2, TOut>(Result<T1> r1, Result<T2> r2, Func<T1, T2, TOut> func) =>
        (r1, r2) switch
        {
            (Success<T1> s1, Success<T2> s2) => Success(func(s1.Value, s2.Value)),
            _ => Combine(r1, r2).Match(
                _ => throw new Exception("Should not happen."),
                errors => Failure<TOut>(errors.ToArray())
            )
        };

    public static Result<TOut> ValuesToObject<T1, T2, T3, TOut>(Result<T1> r1, Result<T2> r2, Result<T3> r3, Func<T1, T2, T3, TOut> func) =>
        (r1, r2, r3) switch
        {
            (Success<T1> s1, Success<T2> s2, Success<T3> s3) => Success(func(s1.Value, s2.Value, s3.Value)),
            _ => Combine(r1, r2, r3).Match(
                _ => throw new Exception("Should not happen."),
                errors => Failure<TOut>(errors.ToArray())
            )
        };


    private static Result<None> Merge(this IEnumerable<Result> all) =>
        all.SelectMany(result =>
                result switch
                {
                    Failure<None> failure => failure.Errors,
                    _ => Array.Empty<ResultError>()
                })
            .ErrorsToSingleResult();

    private static Result<None> ErrorsToSingleResult(this IEnumerable<ResultError> errors) =>
        errors.Any()
            ? Failure<None>(errors.ToArray())
            : Success();


    private static Result<None> None => Success(new None());
}