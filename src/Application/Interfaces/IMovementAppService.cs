using Mttechne.Application.ViewModel;
using Mttechne.Domain.Models;

namespace Mttechne.Application.Interfaces;

public interface IMovementAppService
{
    Task<IEnumerable<MovementViewModel>> GetAllAsync();
    Task<IEnumerable<MovementViewModel>> GetMovimentationFromDayAsync(DateTime date);
    Task<IEnumerable<MovementTypeViewModel>> GetAllTypesAsync();
    Task<IEnumerable<Tuple<DateTime, decimal, int>>> GetTotalizersAsync();
    Task<MovementViewModel> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<MovementViewModel> CreateAsync(MovementViewModel movement);
}