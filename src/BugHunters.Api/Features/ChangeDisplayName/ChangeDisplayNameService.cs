using BugHunters.Api.Common.FunctionalCore;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Features.ChangeDisplayName;

public class ChangeDisplayNameService : ICoreService
{
    public Result<Hunter> ChangeDisplayName(Hunter? hunter, string newDisplayName)
    {
        if (hunter is null)
        {
            return Result<Hunter>.Failure(new("Hunter", "Hunter not found."));
        }

        Result<DisplayName> displayNameResult = DisplayName.FromString(newDisplayName);
        if (displayNameResult.IsFailure)
        {
            return displayNameResult.ToOther<Hunter>();
        }

        DisplayName displayName = displayNameResult.Payload;
        Hunter updated = hunter with { DisplayName = displayName };

        return Result.Success(updated);
    }
}