using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Common.Endpoint;

public static class ApiEndpoint
{
    public abstract class WithRequest<TRequest> : EndpointBase
    {
        public abstract Task<IResult> HandleAsync(TRequest request);
    }

    public abstract class WithoutRequest
    {
        public abstract Task<IResult> HandleAsync();
    }
}

[ApiController, Route("api")]
public abstract class EndpointBase : ControllerBase
{
    public IResult ToProblemDetails(IEnumerable<ResultError> errors) =>
        Results.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            extensions: errors.ToDictionary(error => error.Code, error => (object?)error.Message));
}