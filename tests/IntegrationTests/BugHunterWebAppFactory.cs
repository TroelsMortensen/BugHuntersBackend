using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests;

internal class BugHunterWebAppFactory : WebApplicationFactory<Program>
{
    private IServiceCollection serviceCollection = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // setup extra test services.
        builder.ConfigureTestServices(services =>
        {
            serviceCollection = services;
            // Remove the existing DbContexts and Options
            services.RemoveAll(typeof(DbContextOptions<BugHunterContext>));
            services.RemoveAll<BugHunterContext>();

            string connString = GetConnectionString();
            services.AddDbContext<BugHunterContext>(options => { options.UseSqlite(connString); });

            SetupCleanDatabase(services);
        });
    }

    private static void SetupCleanDatabase(IServiceCollection services)
    {
        BugHunterContext ctx = services.BuildServiceProvider().GetService<BugHunterContext>()!;
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
    }

    private static string GetConnectionString()
    {
        string testDbName = "Test" + Guid.NewGuid() + ".db";
        return "Data Source = " + testDbName;
    }

    protected override void Dispose(bool disposing)
    {
        // clean up the database
        BugHunterContext ctx = serviceCollection.BuildServiceProvider().GetService<BugHunterContext>()!;
        ctx.Database.EnsureDeleted();
        base.Dispose(disposing);
    }
}