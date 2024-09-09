using BugHunters.Api.Entities.Common;
using BugHunters.Api.Entities.HunterEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BugHunters.Api.Persistence.Configurations;

public class HunterConfig : IEntityTypeConfiguration<Hunter>
{
    public void Configure(EntityTypeBuilder<Hunter> builder)
    {
        PkConfig(builder);
        NameConfig(builder);
        ViaIdConfig(builder);
    }

    private void ViaIdConfig(EntityTypeBuilder<Hunter> builder)
        => builder.ComplexProperty<ViaId>(h => h.ViaId,
            propBuilder =>
            {
                propBuilder.Property<string>(v => v.Value)
                    .HasColumnName("ViaId");
            });

    private void NameConfig(EntityTypeBuilder<Hunter> builder)
        => builder.ComplexProperty<DisplayName>(h => h.DisplayName,
            propBuilder =>
            {
                propBuilder.Property<string>(n => n.Value)
                    .HasColumnName("DisplayName");
            });

    private void PkConfig(EntityTypeBuilder<Hunter> builder)
    {
        builder.HasKey(h => h.Id);
    }
}

public class HunterIdConverter() : ValueConverter<Id<Hunter>, Guid>(
    id => id.Value,
    dbValue => Id<Hunter>.FromGuid(dbValue)
);