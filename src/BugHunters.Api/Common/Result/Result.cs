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

    public static Result<T> AssertThat<T>(this Result<T> result, Func<bool> predicate, ResultError error) =>
        result switch
        {
            Failure<T> failure when !predicate() => Failure<T>(failure.Errors.Append(error).ToArray()),
            _ when !predicate() => Failure<T>(error),
            _ when predicate() => result,
            _ => throw new ArgumentException("Unknown type of result.")
        };

    public static Result<T> WithPayloadIfSuccess<T>(this Result<None> result, Func<T> payload) =>
        result switch
        {
            Success<T> => Success(payload()),
            Failure<T> failure => failure,
            _ => throw new ArgumentException("Unknown type of result.")
        };
}