using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class Repository<T>(ApplicationDbContext context) : IRepository<T>
    where T : class
{
    public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken ct = default)
        where TId : notnull
        => await context.Set<T>().FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await context.Set<T>().ToListAsync(ct);

    public void Add(T entity) => context.Set<T>().Add(entity);
    public void Update(T entity) => context.Set<T>().Update(entity);
    public void Delete(T entity) => context.Set<T>().Remove(entity);
}
