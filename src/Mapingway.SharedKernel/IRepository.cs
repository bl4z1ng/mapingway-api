using System.Linq.Expressions;

namespace Mapingway.SharedKernel;

public interface IRepository<TEntity>
{
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken? ct);
    Task<TEntity?> GetByIdAsync(object id, CancellationToken? ct);
    Task CreateAsync(TEntity user, CancellationToken? ct);
    Task DeleteAsync(int id, CancellationToken? ct);
    void Delete(TEntity entity);
    void Update(TEntity entity);
}
