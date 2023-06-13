namespace Mapingway.Common.Repository;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync(CancellationToken ct);
    Task<T?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<T>> GetByConditionAsync(Func<T,bool> condition, CancellationToken ct);
    Task<int> CreateAsync(T entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    void UpdateAsync(T entity, CancellationToken ct);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}