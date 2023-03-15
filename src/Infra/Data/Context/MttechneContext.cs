using Mttechne.Domain.Models;
using Mttechne.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Mttechne.Infra.Data.Context;

public sealed class MttechneContext : DbContext
{
    public MttechneContext(DbContextOptions<MttechneContext> options)
        : base(options)
    {
    }

    public DbSet<Movement> Movements { get; set; }

    public DbSet<MovementType> MovementTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovementMap());
        modelBuilder.ApplyConfiguration(new MovementTypeMap());
        MovementType.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}