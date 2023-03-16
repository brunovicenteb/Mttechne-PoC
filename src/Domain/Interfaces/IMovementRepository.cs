using Mttechne.Domain.Models;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Domain.Interfaces;

public interface IMovementRepository : IBaseRepository<Movement>
{
    Task<IEnumerable<MovementType>> GetAllTypesAsync();

    Task<IEnumerable<Movement>> GetMovimentationFromDayAsync(DateTime date);

    Task<IEnumerable<Tuple<DateTime, decimal, int>>> GetTotalizersAsync();
}