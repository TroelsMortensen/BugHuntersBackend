using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Persistence;

public class BugHunterContext(DbContextOptions<BugHunterContext> options) : DbContext(options)
{
    public DbSet<Hunter> Hunters => Set<Hunter>();

    public DbSet<Bug> Bugs => Set<Bug>();

    public DbSet<BugCatch> BugCatches => Set<BugCatch>();

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