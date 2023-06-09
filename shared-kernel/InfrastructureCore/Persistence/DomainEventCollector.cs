using Microsoft.EntityFrameworkCore;
using PlatformPlatform.SharedKernel.DomainCore.DomainEvents;
using PlatformPlatform.SharedKernel.DomainCore.Entities;

namespace PlatformPlatform.SharedKernel.InfrastructureCore.Persistence;

public sealed class DomainEventCollector : IDomainEventCollector
{
    private readonly DbContext _dbContext;

    public DomainEventCollector(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<IAggregateRoot> GetAggregatesWithDomainEvents()
    {
        return _dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);
    }
}