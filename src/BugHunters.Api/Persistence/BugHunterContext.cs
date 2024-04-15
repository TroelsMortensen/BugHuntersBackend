using BugHunters.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Persistence;

public class BugHunterContext(DbContextOptions<BugHunterContext> options) : DbContext(options)
{
    public DbSet<Hunter> Hunters { get; set; }

    public DbSet<Bug> Bugs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BugHunterContext).Assembly);
    }
}