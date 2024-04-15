using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.Hunter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugHunters.Api.Persistence.HunterConfigurations;

public class HunterConfiguration : IEntityTypeConfiguration<Hunter>
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
        => builder.ComplexProperty<Name>(h => h.Name,
            propBuilder =>
            {
                propBuilder.Property<string>(n => n.Value)
                    .HasColumnName("Name");
            });

    private void PkConfig(EntityTypeBuilder<Hunter> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasConversion(
            id => id.Value,
            dbValue => Id<Hunter>.FromGuid(dbValue)
        );
    }
}