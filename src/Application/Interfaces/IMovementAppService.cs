using Mttechne.Application.ViewModel;
using Mttechne.Domain.Models;

namespace Mttechne.Application.Interfaces;

public interface IMovementAppService
{
    Task<IEnumerable<MovementViewModel>> GetAllAsync();
    Task<IEnumerable<MovementViewModel>> GetTodayMovimentationAsync();
    Task<IEnumerable<MovementTypeViewModel>> GetAllTypesAsync();
    Task<IEnumerable<Tuple<DateTime, decimal>>> GetTotalizersAsync();
    Task<MovementViewModel> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<MovementViewModel> CreateAsync(MovementViewModel movement);
}