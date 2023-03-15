using Mttechne.Domain.Models;
using Mttechne.Domain.Interfaces;
using Mttechne.Infra.Data.Context;
using Mttechne.Toolkit.RelationalDb;
using Microsoft.EntityFrameworkCore;

namespace Mttechne.Infra.Data.Repository;

public class MovementRepository : RelationalDbRepository<MttechneContext, Movement>, IMovementRepository
{
    public MovementRepository(MttechneContext context)
        : base(context)
    {
    }

    protected override DbSet<Movement> Collection
        => Context.Movements;

    public override Task<Movement> AddAsync(Movement entity, bool applySave = true)
    {
        entity.CreatedAt = null;
        return base.AddAsync(entity, applySave);
    }

    public async override Task<Movement> GetObjectByIDAsync(int id)
        => await Collection
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id && !o.DeletedAt.HasValue);

    public override async Task<IEnumerable<Movement>> GetAllAsync()
        => await Collection
            .Where(o => !o.DeletedAt.HasValue)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .Include(o => o.MovementType)
            .ToListAsync();

    public async Task<IEnumerable<MovementType>> GetAllTypesAsync()
        => await Context.MovementTypes
            .OrderByDescending(o => o.Name)
            .AsNoTracking()
            .ToListAsync();
}