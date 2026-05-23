using Microsoft.EntityFrameworkCore;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Application.Common.Contracts.Outbox;

namespace AnkiBridge.Infrastructure.Outbox;

public class OutboxMessageRepository(
    ApplicationDbContext context) 
    : IOutboxMessageRepository
{
    private const int MaxRetries = 3;

    public async Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(
    CancellationToken cancellationToken = default)
    {
        return await context.OutboxMessages
            .Where(m => m.ProcessedDate == null && m.ProcessedCount < MaxRetries)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        OutboxMessage message, 
        CancellationToken cancellationToken = default )
    {
        await context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public void MarkAsFailed(OutboxMessage message, bool recoverable = true)
    {
        if (recoverable)
            message.ProcessedCount++;
        else
            message.ProcessedCount = MaxRetries;
    }

    public void MarkAsProcessed(OutboxMessage message)
    {
        message.ProcessedCount++;
        message.ProcessedDate = DateTime.UtcNow;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
