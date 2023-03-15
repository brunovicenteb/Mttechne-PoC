using Mttechne.Domain.Interfaces;
using Mttechne.Application.ViewModel;
using AutoMapper;
using Mttechne.Domain.Models;
using Mttechne.Toolkit.Mapper;
using MassTransit.Initializers;

namespace Mttechne.Application.Interfaces;

public class MovementAppService : IMovementAppService
{
    public MovementAppService(IMovementRepository repository)
    {
        _repository = repository;
    }

    private readonly IMovementRepository _repository;

    public Task<bool> DeleteAsync(int id)
        => _repository.DeleteAsync(id);

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
        var entity = await _repository.GetAllAsync();
        var mapper = MapperFactory.Map<Movement, MovementViewModel>();
        return entity
            .Select(o =>
            {
                var mapped = mapper.Map<Movement, MovementViewModel>(o);
                mapped.MovementType = o.MovementType.Name;
                return mapped;
            });
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
}