using FluentValidation;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.Data;

public abstract class BaseEntity : IIdentifiable
{
    // Empty constructor for EF
    public BaseEntity() : base() { }

    public BaseEntity(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public virtual IValidator[] GetValidators()
        => Array.Empty<IValidator>();
}