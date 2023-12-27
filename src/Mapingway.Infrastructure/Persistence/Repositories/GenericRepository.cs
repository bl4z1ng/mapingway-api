using System.Linq.Expressions;
using Mapingway.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;


    protected GenericRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }


    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken ct = default)
    {
        var query = (IQueryable<TEntity>)DbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(ct);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object? id, CancellationToken ct = default)
    {
        return await DbSet.FindAsync([id], cancellationToken: ct);
    }

    public virtual async Task CreateAsync(TEntity user, CancellationToken ct = default)
    {
        await DbSet.AddAsync(user, ct);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Attach(entity);
        DbSet.Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await DbSet.FindAsync([id], cancellationToken: ct);

        if (entity is not null)
            Delete(entity);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (DbSet.Entry(entityToDelete).State == EntityState.Detached)
        {
            DbSet.Attach(entityToDelete);
        }

        DbSet.Remove(entityToDelete);
    }
}
