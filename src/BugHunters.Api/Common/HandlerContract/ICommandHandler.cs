namespace BugHunters.Api.Common.HandlerContract;

public interface ICommandHandler<in TCommand>
{
    Task<Result<None>> HandleAsync(TCommand command);
}