namespace BugHunters.Api.Common.HandlerContracts;

public interface IQueryHandler<in TQuery, TAnswer>
{
    Task<Result<TAnswer>> HandleAsync(TQuery command);
}