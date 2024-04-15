namespace BugHunters.Api.Common.HandlerContract;

public interface IQueryHandler<in TQuery, TAnswer>
{
    Task<Result<TAnswer>> HandleAsync(TQuery command);
}