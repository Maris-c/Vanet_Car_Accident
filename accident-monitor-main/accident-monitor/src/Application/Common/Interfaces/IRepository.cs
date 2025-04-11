namespace AccidentMonitor.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<T?> DeleteAsync(Guid id);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
