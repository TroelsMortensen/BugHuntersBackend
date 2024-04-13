using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BugHunters.Api.Persistence;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BugHunterContext>
{
    public BugHunterContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BugHunterContext>();
        optionsBuilder.UseSqlite(@"Data Source = VEADatabaseProduction.db");
        return new BugHunterContext(optionsBuilder.Options);
    }
}