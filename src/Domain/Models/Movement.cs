using Microsoft.EntityFrameworkCore;
using Mttechne.Toolkit.Data;

namespace Mttechne.Domain.Models;

public class Movement : LifeTimeEntity
{
    private static readonly string[] _FirstNames = { "Pay", "Rent", "Sale", "Loan", "Take" };
    private static readonly string[] _LastNames = { "Car", "Credicard", "Bike", "Phone", "Document", "Pictures" };
    protected Movement() { }

    public Movement(int id, int movementTypeId, decimal value)
        : base()
    {
        Id = id;
        MovementTypeId = movementTypeId;
        Value = value;
    }

    public int MovementTypeId { get; private set; }
    public MovementType MovementType { get; private set; }
    public decimal Value { get; private set; }
    public string Description { get; private set; }

    public static void Seed(ModelBuilder modelBuilder)
    {
        int count = 0;
        var seeded = new List<Movement>();
        DateTime current = DateTime.Now;
        int daysCount = Random.Shared.Next(2, 4) * 30;
        while (daysCount > 0)
        {
            daysCount--;
            int movementCount = Random.Shared.Next(10, 15);
            for (int i = 0; i < movementCount; i++)
            {
                var moment = new DateTime(current.Year, current.Month, current.Day,
                    Random.Shared.Next(0, 24), Random.Shared.Next(0, 59), Random.Shared.Next(0, 59));
                if (moment >= DateTime.Now)
                    moment = moment.AddHours(-1);
                var name = GenerateName();
                var value = Convert.ToDecimal(Random.Shared.NextDouble() * Random.Shared.Next(1, 1500));
                seeded.Add(new Movement
                {
                    Id = ++count,
                    CreatedAt = moment,
                    Description = name,
                    Value = Random.Shared.Next(0, 1000) % 2 == 0 ? value : value *= -1,
                    MovementTypeId = Random.Shared.Next(0, 1000) % 2 == 0 ? MovementType.CreditId : MovementType.DebitId
                });
            }
            current = current.AddDays(-1);
        }
        modelBuilder.Entity<Movement>().HasData(seeded.ToArray());
    }

    public static string GenerateName()
    {
        var random = new Random();
        string firstName = _FirstNames[random.Next(0, _FirstNames.Length)];
        string lastName = _LastNames[random.Next(0, _LastNames.Length)];
        return $"{firstName} {lastName}";
    }
}