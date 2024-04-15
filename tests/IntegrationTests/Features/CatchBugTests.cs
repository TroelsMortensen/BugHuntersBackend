﻿using System.Net;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.Hunter;
using BugHunters.Api.Entities.Values.StrongId;
using BugHunters.Api.Features.CatchBug;
using BugHunters.Api.Persistence;

namespace IntegrationTests.Features;

public class CatchBugTests
{
    [Fact]
    public async Task ShouldCatchBug()
    {
        await using BugHunterWebAppFactory waf = new();
        Id<Bug> bugId = await InsertBug(waf);
        Id<Hunter> hunterId = await InsertHunter(waf);
        HttpClient client = waf.CreateClient();

        CatchBugRequest request = new CatchBugRequest(bugId.Value.ToString(), hunterId.Value.ToString());

        HttpResponseMessage httpResponse = await client.PostAsync("/api/catch-bug", JsonContent.Create(request));
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
    }

    private async Task<Id<Hunter>> InsertHunter(BugHunterWebAppFactory waf)
    {
        Id<Hunter> hunterId = Id<Hunter>.New();
        Hunter hunter = new Hunter(
            hunterId,
            Name.FromString("Troels").EnsureValidResult().Payload,
            ViaId.FromString("trmo").EnsureValidResult().Payload
        );
        await using BugHunterContext ctx = waf.Services.CreateScope().ServiceProvider.GetRequiredService<BugHunterContext>();
        await ctx.Hunters.AddAsync(hunter);
        await ctx.SaveChangesAsync();
        return hunterId;
    }

    private static async Task<Id<Bug>> InsertBug(BugHunterWebAppFactory waf)
    {
        Id<Bug> bugId = Id<Bug>.New();
        Bug bug = new Bug(bugId, "TestBug", "This is a test bug", "Placed right here", new byte[1]);
        await using BugHunterContext ctx = waf.Services.CreateScope().ServiceProvider.GetRequiredService<BugHunterContext>();
        await ctx.Bugs.AddAsync(bug);
        await ctx.SaveChangesAsync();
        return bugId;
    }
}