using BugHunters.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugHunters.Api.Persistence.Configurations;

public class BugCatchConfig : IEntityTypeConfiguration<BugCatch>
{
    public void Configure(EntityTypeBuilder<BugCatch> builder)
    {
        PkConfig(builder);
    }

    private void PkConfig(EntityTypeBuilder<BugCatch> builder)
    {
        builder.HasKey(bc => new { bc.CaughtBy, bc.BugCaught });
    }
}

