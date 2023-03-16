using Mttechne.Domain.Models;
using Mttechne.Toolkit.Mapper;
using MassTransit.Initializers;
using Mttechne.Domain.Interfaces;
using Mttechne.Application.ViewModel;

namespace Mttechne.Application.Interfaces;

public class MovementAppService : IMovementAppService
{
    public MovementAppService(IMovementRepository repository)
    {
        _repository = repository;
    }

    private readonly IMovementRepository _repository;

    public async Task<bool> DeleteAsync(int id)
        => await _repository.DeleteAsync(id);


    public async Task<IEnumerable<Tuple<DateTime, decimal, int>>> GetTotalizersAsync()
        => await _repository.GetTotalizersAsync();

    public async Task<MovementViewModel> CreateAsync(MovementViewModel movement)
    {
        if (movement.MovementTypeId != MovementType.CreditId)
            movement.Value = movement.Value *= -1;
        var mapper = MapperFactory.Map<MovementViewModel, Movement>();
        var entity = mapper.Map<MovementViewModel, Movement>(movement);
        var reloadedEntity = await _repository.AddAsync(entity);
        var mapperReverse = MapperFactory.Map<Movement, MovementViewModel>();
        var reloadedModel = mapperReverse.Map<Movement, MovementViewModel>(reloadedEntity);
        return reloadedModel;
    }

    public async Task<IEnumerable<MovementViewModel>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return ConvertToModel(entities);
    }

    public async Task<IEnumerable<MovementViewModel>> GetTodayMovimentationAsync()
    {
        var entities = await _repository.GetTodayMovimentationAsync();
        return ConvertToModel(entities);
    }

    public async Task<MovementViewModel> GetByIdAsync(int id)
    {
        var entity = await _repository.GetObjectByIDAsync(id);
        var mapper = MapperFactory.Map<Movement, MovementViewModel>();
        var loadedModel = mapper.Map<Movement, MovementViewModel>(entity);
        return loadedModel;
    }

    public async Task<IEnumerable<MovementTypeViewModel>> GetAllTypesAsync()
    {
        var entities = await _repository.GetAllTypesAsync();
        var mapper = MapperFactory.Map<MovementType, MovementTypeViewModel>();
        var loadedModels = entities
            .Select(o => mapper.Map<MovementType, MovementTypeViewModel>(o));
        return loadedModels;
    }

    private IEnumerable<MovementViewModel> ConvertToModel(IEnumerable<Movement> movements)
    {
        var mapper = MapperFactory.Map<Movement, MovementViewModel>();
        return movements
            .Select(o =>
            {
                var mapped = mapper.Map<Movement, MovementViewModel>(o);
                mapped.MovementType = o.MovementType.Name;
                return mapped;
            });
    }
}