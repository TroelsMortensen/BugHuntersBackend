namespace BugHunters.Api.Common.HandlerContracts;

public interface ICommandHandler<in TCommand>
{
    Task<Result<None>> HandleAsync(TCommand command);
}