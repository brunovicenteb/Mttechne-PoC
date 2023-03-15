using Mttechne.Toolkit.Exceptions;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.Data;

public abstract class LifeTimeEntity : BaseEntity, ILifeTime
{
    // Empty constructor for EF
    public LifeTimeEntity() : base() { }

    public LifeTimeEntity(DateTime? createdAt, DateTime? updatedAt, DateTime? deletedAt)
        : base()
    {
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActive => !DeletedAt.HasValue;

    public bool Update(DateTime? atualizadoEm = null)
    {
        UpdatedAt = atualizadoEm ?? DateTime.UtcNow;
        return true;
    }

    public bool Delete()
    {
        if (DeletedAt.HasValue)
            throw new DomainRuleException("Entity already deleted.");
        DeletedAt = DateTime.UtcNow;
        return true;
    }
}