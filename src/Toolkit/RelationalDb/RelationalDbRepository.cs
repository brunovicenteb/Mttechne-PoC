using Mttechne.Toolkit.Data;
using Mttechne.Toolkit.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mttechne.Toolkit.RelationalDb;

public abstract class RelationalDbRepository<TContext, TEntity> : IBaseRepository<TEntity>
    where TContext : DbContext
    where TEntity : BaseEntity
{
    public RelationalDbRepository(TContext context)
    {
        Context = context;
    }

    public readonly TContext Context;
    protected abstract DbSet<TEntity> Collection { get; }
    public abstract Task<TEntity> GetObjectByIDAsync(int id);
    public abstract Task<IEnumerable<TEntity>> GetAllAsync();

    public async Task<long> CountAsync()
    {
        return await Collection.CountAsync();
    }

    public async Task<bool> DeleteAsync(int id, bool applySave = true)
    {
        TEntity entity = await GetObjectByIDAsync(id);
        Collection.Remove(entity);
        if (!applySave)
            return true;
        return await Context.SaveChangesAsync() == 1;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, bool applySave = true)
    {
        await Collection.AddAsync(entity);
        if (applySave)
            await Context.SaveChangesAsync();
        return await GetObjectByIDAsync(entity.Id);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, bool applySave = true)
    {
        Collection.Update(entity);
        if (applySave)
            await Context.SaveChangesAsync();
        return await GetObjectByIDAsync(entity.Id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        int count = await Context.SaveChangesAsync();
        return count > 0;
    }
}