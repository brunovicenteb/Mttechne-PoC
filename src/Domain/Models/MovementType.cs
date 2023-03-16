using Microsoft.EntityFrameworkCore;
using Mttechne.Toolkit.Data;

namespace Mttechne.Domain.Models;

public class MovementType : BaseEntity
{
    public const int CreditId = 1;
    public const int DebitId = 2;

    public MovementType(int id, string name)
    {
        Id = id;
        Name = name;
    }

    // Empty constructor for EF
    protected MovementType() { }

    public string Name { get; private set; }

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovementType>().HasData(
            new MovementType { Id = CreditId, Name = "Credit" },
            new MovementType { Id = DebitId, Name = "Debit" }
            );
    }
}