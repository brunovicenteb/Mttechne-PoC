using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mttechne.Toolkit.Data.Config;

public abstract class CicloDeVidaConfig<T> : BaseEntityConfig<T> where T : LifeTimeEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnUpdate();
    }
}