using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AnkiBridge.Infrastructure.Persistence.Interceptors;

public sealed class DomainEventDispatchInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not ApplicationDbContext context)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEntities = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents is not null && x.Entity.DomainEvents.Count > 0);

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
