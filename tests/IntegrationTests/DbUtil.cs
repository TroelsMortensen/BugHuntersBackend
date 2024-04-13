using BugHunters.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public static class DbUtil
{
    internal static BugHunterContext SetupContext()
    {
        DbContextOptionsBuilder<BugHunterContext> optionsBuilder = new();
        string testDbName = "Test" + Guid.NewGuid() + ".db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDbName);
        BugHunterContext context = new(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}