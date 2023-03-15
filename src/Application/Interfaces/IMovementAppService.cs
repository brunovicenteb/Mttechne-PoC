using Mttechne.Application.ViewModel;

namespace Mttechne.Application.Interfaces;

public interface IMovementAppService
{
    Task<IEnumerable<MovementViewModel>> GetAllAsync();
    Task<IEnumerable<MovementTypeViewModel>> GetAllTypesAsync();
    Task<MovementViewModel> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<MovementViewModel> CreateAsync(MovementViewModel movement);
}