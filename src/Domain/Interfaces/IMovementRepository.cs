using Mttechne.Domain.Models;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Domain.Interfaces;

public interface IMovementRepository : IBaseRepository<Movement>
{
    Task<IEnumerable<MovementType>> GetAllTypesAsync();
}