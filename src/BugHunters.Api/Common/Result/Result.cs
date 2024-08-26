using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Common.Result;

public record Result<T>
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

    private static Result<None> None => Success(new None());

    public static Result<None> StartValidation() =>
        None;

    public static Result<None> AssertThat(this Result<None> result, Func<Result<None>> func) =>
        new List<Result<None>>
            {
                result, func()
            }
            .Merge();

    private static Result<None> Merge(this IEnumerable<Result<None>> all) =>
        all.SelectMany(result =>
                result switch
                {
                    Failure<None> failure => failure.Errors,
                    _ => Array.Empty<ResultError>()
                })
            .ToSingleResult();

    private static Result<None> ToSingleResult(this IEnumerable<ResultError> errors) =>
        errors.Any()
            ? Failure<None>(errors.ToArray())
            : Success();

    public static Result<T> WithPayloadIfSuccess<T>(this Result<None> result, Func<T> payload) =>
        result switch
        {
            Success<T> => Success(payload()),
            Failure<T> failure => failure,
            _ => throw new ArgumentException("Unknown type of result.")
        };
}