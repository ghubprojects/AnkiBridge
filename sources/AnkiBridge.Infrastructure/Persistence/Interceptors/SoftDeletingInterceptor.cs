using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AnkiBridge.Infrastructure.Persistence.Interceptors;

public sealed class SoftDeletingInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not ApplicationDbContext context)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeleteEntity>())
        {
            if (entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;
            entry.Property(nameof(ISoftDeleteEntity.IsDeleted)).CurrentValue = true;
            entry.Property(nameof(ISoftDeleteEntity.DeletedAt)).CurrentValue = utcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}