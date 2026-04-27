using LexiBridge.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LexiBridge.Infrastructure.Persistence.Interceptors;

public sealed class AuditingInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            var now = DateTimeOffset.UtcNow;
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = now;
                    break;
                case EntityState.Modified:
                    entry.Property(nameof(IAuditableEntity.LastModifiedAt)).CurrentValue = now;
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}