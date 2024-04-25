using System.Collections.Immutable;
using System.Text.Json;

namespace BugHunters.Api.Common.Result;

public class Result<T> : Result, IFluentResultValidation<T>
{
    private readonly T payload = default!;

    public T Payload
    {
        get
        {
            AssertAccessible();
            return payload;
        }
    }

    private void AssertAccessible()
    {
        if (Errors.Any()) throw new OperationResultHasErrors();
        if (ValueIsDefault()) throw new PayloadIsEmpty();
        if (new None().Equals(payload)) throw new OperationResultContainsNone();
    }

    private bool ValueIsDefault()
        => EqualityComparer<T>.Default.Equals(payload, default);

    public bool IsFailure => errors.Count != 0 || ValueIsDefault();

    public bool IsSuccess => !IsFailure;

    public Result<TOther> ToOther<TOther>()
        => Errors.ToList();

    public static implicit operator Result<T>(T value)
        => new(value);

    public static implicit operator Result<T>(ResultError error)
        => new(error);

    public static implicit operator Result<T>(List<ResultError> errors)
    {
        Result<T> res = new Result<T>();
        res.errors.AddRange(errors);
        return res;
    }

    internal Result(T payload)
        => this.payload = payload;

    private Result(ResultError resultError)
        => errors.Add(resultError);

    internal Result()
    {
    }

    // This is for testing purposes. It should not be used in production code.
    public Result<T> EnsureValidResult()
    {
        if (IsFailure)
        {
            throw new Exception("Invalid result, you messed up: " + JsonSerializer.Serialize(errors, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        return this;
    }

    public Result<T> WithPayloadIfSuccess(Func<T> instantiatePayload)
        => errors.Count != 0 ? this : instantiatePayload();


    public IFluentResultValidation<T> AssertThat(Func<bool> validation, ResultError error)
    {
        if (!validation())
        {
            errors.Add(error);
        }

        return this;
    }

    public Result<T> ToResult() => this;

    public static Result<T> Failure(ResultError resultError)
    {
        Result<T> result = new Result<T>();
        result.errors.Add(resultError);
        return result;
    }
}

public abstract class Result
{
    public IEnumerable<ResultError> Errors => errors.ToImmutableList();

    protected readonly List<ResultError> errors = [];


    public static Result<T> Success<T>(T value)
        => new(value);

    public static Result<None> Success()
        => new(new None());

    // public static Result<T> Failure<T>(string errorCode, string message)
    //     => new(new ResultError(errorCode, message));

    public static Result<None> CombineResults(params Result[] results)
    {
        Result<None> result = results.SelectMany(r => r.Errors).ToList();
        return result.WithPayloadIfSuccess(() => new None());
    }

    public static Result<T> CombineResultsInto<T>(params Result[] results)
        => results.SelectMany(r => r.Errors).ToList();


    public static IFluentResultValidation<None> StartValidation()
        => Success();

    public static IFluentResultValidation<T> StartValidation<T>()
        => new Result<T>();
}

public class OperationResultHasErrors() : Exception("Cannot access result value, if the result has errors");

public class PayloadIsEmpty() : Exception("Result value not set");

public class OperationResultContainsNone() : Exception("Result contains none");

public record ResultError(string Code, string Message);

public record None;

public interface IFluentResultValidation<T>
{
    IFluentResultValidation<T> AssertThat(Func<bool> validation, ResultError error);
    public Result<T> WithPayloadIfSuccess(Func<T> instantiatePayload);
}