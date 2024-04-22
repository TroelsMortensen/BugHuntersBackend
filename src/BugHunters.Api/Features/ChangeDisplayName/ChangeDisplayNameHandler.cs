using BugHunters.Api.Common.HandlerContracts;
using BugHunters.Api.Entities;
using BugHunters.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Features.ChangeDisplayName;

public class ChangeDisplayNameHandler(BugHunterContext context, ChangeDisplayNameService service) : ICommandHandler<ChangeDisplayNameCommand>
{
    public async Task<Result<None>> HandleAsync(ChangeDisplayNameCommand command)
    {
        Result<Id<Hunter>> idResult = Id<Hunter>.FromString(command.HunterId);
        if (idResult.IsFailure)
        {
            return new ResultError("HunterId", "Invalid HunterId format."); 
        }

        Hunter? hunter = await context.Hunters.SingleOrDefaultAsync(h => h.Id == idResult.Payload);
        Result<Hunter> updatedHunterResult = service.ChangeDisplayName(hunter, command.NewDisplayName);
        
        if(updatedHunterResult.IsFailure)
        {
            return updatedHunterResult.Errors.ToList();
        }

        context.Hunters.Update(updatedHunterResult.Payload);
        await context.SaveChangesAsync();
        return Result.Success();
    }
}