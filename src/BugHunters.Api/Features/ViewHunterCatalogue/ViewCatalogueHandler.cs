using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Entities;
using BugHunters.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Features.ViewHunterCatalogue;

public class ViewCatalogueHandler(BugHunterContext context) : IQueryHandler<ViewCatalogueQuery, ViewCatalogueAnswer>
{
    public async Task<Result<ViewCatalogueAnswer>> HandleAsync(ViewCatalogueQuery command)
    {
        Result<Id<Hunter>> hunterIdResult = Id<Hunter>.FromString(command.HunterId);
        if (hunterIdResult.IsFailure)
        {
            return Result<ViewCatalogueAnswer>.Failure(new ResultError("Hunter.Id", "Invalid HunterId"));
        }

        Hunter? hunter = await context.Hunters.SingleOrDefaultAsync(h => h.Id == hunterIdResult.Payload);
        if (hunter is null)
        {
            return Result<ViewCatalogueAnswer>.Failure(new ResultError("Hunter.Id", "Hunter not found"));
        }

        List<BugDto> bugs = await context.BugCatches
            .Where(x => x.HunterId == hunterIdResult.Payload)
            .OrderBy(x => x.TimeCaught)
            .Select(x => new BugDto(x.Bug.Name, x.Bug.Description, x.Bug.Image))
            .ToListAsync();

        return new ViewCatalogueAnswer(bugs);
    }
}