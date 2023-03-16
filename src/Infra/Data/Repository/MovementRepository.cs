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

    public async Task<IEnumerable<Movement>> GetTodayMovimentationAsync()
    {
        var today = DateTime.Today.Date;
        return await Collection
                    .Where(b => !b.DeletedAt.HasValue && (b.CreatedAt.Value.Year == today.Year
                                                      && b.CreatedAt.Value.Month == today.Month
                                                      && b.CreatedAt.Value.Day == today.Day))
                    .OrderByDescending(o => o.CreatedAt)
                    .AsNoTracking()
                    .Include(o => o.MovementType)
                            .ToListAsync();
    }

    public async Task<IEnumerable<Tuple<DateTime, decimal>>> GetTotalizersAsync()
    {
        var resultSet = await GetAllAsync();
        return resultSet
            .GroupBy(row => new { row.CreatedAt.Value.Date })
            .Select(g => new Tuple<DateTime, decimal>(g.Key.Date, g.Sum(o => o.Value)))
            .OrderByDescending(o => o.Item1);
    }

    public async Task<IEnumerable<MovementType>> GetAllTypesAsync()
        => await Context.MovementTypes
            .OrderByDescending(o => o.Name)
            .AsNoTracking()
            .ToListAsync();
}