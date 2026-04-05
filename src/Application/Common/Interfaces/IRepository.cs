namespace Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken ct = default) where TId : notnull;
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
