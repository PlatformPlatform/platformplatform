using Microsoft.EntityFrameworkCore;
using PlatformPlatform.Foundation.DomainModeling.Entities;
using PlatformPlatform.Foundation.DomainModeling.Persistence;

namespace PlatformPlatform.Foundation.InfrastructureCore.Persistence;

public abstract class RepositoryBase<T, TId> : IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : IComparable<TId>
{
    protected readonly DbSet<T> DbSet;

    protected RepositoryBase(DbContext context)
    {
        DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var keyValues = new object?[] {id};
        return await DbSet.FindAsync(keyValues, cancellationToken);
    }

    public void Add(T aggregate)
    {
        if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
        DbSet.Add(aggregate);
    }

    public void Update(T aggregate)
    {
        if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
        DbSet.Update(aggregate);
    }

    public void Remove(T aggregate)
    {
        if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
        DbSet.Remove(aggregate);
    }
}