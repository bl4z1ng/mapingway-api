using System.Linq.Expressions;

namespace Mapingway.SharedKernel;

public interface IRepository<TEntity>
{
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default);
    Task CreateAsync(TEntity user, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    void Delete(TEntity entity);
    void Update(TEntity entity);
}
