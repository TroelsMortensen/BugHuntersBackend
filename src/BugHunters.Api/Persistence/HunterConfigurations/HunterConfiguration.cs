using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values.Hunter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugHunters.Api.Persistence.HunterConfigurations;

public class HunterConfiguration : IEntityTypeConfiguration<Hunter>
{
    public void Configure(EntityTypeBuilder<Hunter> builder)
    {
        PkConfig(builder);
    }

    private void PkConfig(EntityTypeBuilder<Hunter> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasConversion(
            id => id.Value,
            dbValue => HunterId.FromGuid(dbValue).Payload
        );
    }
}