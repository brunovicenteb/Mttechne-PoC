using Mttechne.Domain.Models;
using Mttechne.Toolkit.Data.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mttechne.Infra.Data.Mappings;

public class MovementMap : IEntityTypeConfiguration<Movement>
{
    public void Configure(EntityTypeBuilder<Movement> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id)
            .UseIdentityAlwaysColumn();
        builder.Property(e => e.Description)
            .AddDefaultVarChar(250);
        builder.AddForeingKey(o => o.MovementType, o => o.MovementTypeId);
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnUpdate();
        builder.Property(e => e.DeletedAt)
            .HasColumnType("timestamp");
    }
}