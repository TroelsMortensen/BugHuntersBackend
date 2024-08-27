using System.Net;
using BugHunters.Api.Common.Result;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values.Hunter;
using BugHunters.Api.Entities.Values.StrongId;
using BugHunters.Api.Features.CatchBug;
using BugHunters.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Features;

public class CatchBugTests
{
    [Fact]
    public async Task CatchBug_ValidHunterAndBug_ShouldReturnOk()
    {
        await using BugHunterWebAppFactory waf = new();
        Id<Bug> bugId = await AddValidBug(waf);
        Id<Hunter> hunterId = await AddValidHunter(waf);
        HttpClient client = waf.CreateClient();

        CatchBugRequest request = new(bugId.Value.ToString(), hunterId.Value.ToString());

        HttpResponseMessage httpResponse = await client.PostAsync("/api/catch-bug", JsonContent.Create(request));
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        await using BugHunterContext ctx = waf.Services.CreateScope().ServiceProvider.GetRequiredService<BugHunterContext>();
        BugCatch? bugCatch = await ctx.BugCatches.SingleOrDefaultAsync();
        Assert.NotNull(bugCatch);
        Assert.Equal(bugId, bugCatch.BugId);
        Assert.Equal(hunterId, bugCatch.HunterId);
    }

    [Fact]
    public async Task CatchBug_HunterDoesNotExist_ShouldReturnBadRequest()
    {
    }

    [Fact]
    public async Task CatchBug_BugDoesNotExist_ShouldReturnBadRequest()
    {
    }

    [Fact]
    public async Task CatchBug_BugIsAlreadyCaught_ShouldReturnBadRequest()
    {
    }

    private static async Task<Id<Hunter>> AddValidHunter(BugHunterWebAppFactory waf)
    {
        Id<Hunter> hunterId = Id<Hunter>.New();
        Hunter hunter = new(
            hunterId,
            DisplayName.FromString("Troels").ForceValue(),
            ViaId.FromString("trmo").ForceValue()
        );
        await using BugHunterContext ctx = waf.Services.CreateScope().ServiceProvider.GetRequiredService<BugHunterContext>();
        await ctx.Hunters.AddAsync(hunter);
        await ctx.SaveChangesAsync();
        return hunterId;
    }

    private static async Task<Id<Bug>> AddValidBug(BugHunterWebAppFactory waf)
    {
        Id<Bug> bugId = Id<Bug>.New();
        Bug bug = new(bugId, "TestBug", "This is a test bug", "Placed nowhere", new byte[1]);
        await using BugHunterContext ctx = waf.Services.CreateScope().ServiceProvider.GetRequiredService<BugHunterContext>();
        await ctx.Bugs.AddAsync(bug);
        await ctx.SaveChangesAsync();
        return bugId;
    }
}