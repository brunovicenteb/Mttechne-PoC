using Mttechne.Toolkit.Data;

namespace Mttechne.Domain.Models;

public class Movement : LifeTimeEntity
{
    // Empty constructor for EF
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
}