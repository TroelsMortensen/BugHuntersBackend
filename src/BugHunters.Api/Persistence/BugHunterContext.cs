using System.Linq.Expressions;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.StrongId;
using BugHunters.Api.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Persistence;

public class BugHunterContext(DbContextOptions<BugHunterContext> options) : DbContext(options)
{
    public DbSet<Hunter> Hunters => Set<Hunter>();

    public DbSet<Bug> Bugs => Set<Bug>();

    public DbSet<BugCatch> BugCatches => Set<BugCatch>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BugHunterContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Id<Hunter>>()
            .HaveConversion<HunterIdConverter>();

        configurationBuilder
            .Properties<Id<Bug>>()
            .HaveConversion<BugIdConverter>();
    }
}

public static class ContextExt
{
    public static async Task<Result<None>> TrySaveChangesAsync(this BugHunterContext context)
    {
        try
        {
            await context.SaveChangesAsync();
            return Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Failure(new ResultError("SaveChangesFailed", e.Message));
        }
    }

    public static async Task<Result<T>> SingleOrFailureAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
    {
        T? entity = await queryable.SingleOrDefaultAsync(predicate);
        return entity is null
            ? Failure<T>(new ResultError("EntityNotFound", "Entity not found."))
            : Success(entity);
    }

    public static async Task<Result<T>> HunterExists<T>(this BugHunterContext context, Id<Hunter> hunterId, T toReturn) =>
        await context.Hunters.AnyAsync(h => h.Id == hunterId)
            ? Success(toReturn)
            : Failure<T>(new ResultError("HunterNotFound", "Hunter not found."));
}