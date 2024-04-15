using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BugHunters.Api.Persistence.Configurations;

public class BugConfig : IEntityTypeConfiguration<Bug>
{
    public void Configure(EntityTypeBuilder<Bug> builder)
    {
        PkConfig(builder);
    }

    private void PkConfig(EntityTypeBuilder<Bug> builder)
    {
        builder.HasKey(h => h.Id);
    }
}

public class BugIdConverter() : ValueConverter<Id<Bug>, Guid>(
    id => id.Value,
    dbValue => Id<Bug>.FromGuid(dbValue)
);