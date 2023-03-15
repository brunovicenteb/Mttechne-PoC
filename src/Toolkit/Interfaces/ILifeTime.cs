namespace Mttechne.Toolkit.Interfaces;

public interface ILifeTime
{
    DateTime? CreatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsActive { get; }
    bool Delete();
}