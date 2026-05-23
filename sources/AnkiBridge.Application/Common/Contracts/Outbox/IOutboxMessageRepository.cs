namespace AnkiBridge.Application.Common.Contracts.Outbox;

public interface IOutboxMessageRepository
{
    Task<IEnumerable<OutboxMessage>> GetUnprocessedMessagesAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        OutboxMessage message, 
        CancellationToken cancellationToken = default);

    void MarkAsProcessed(OutboxMessage message);

    void MarkAsFailed(
        OutboxMessage message, 
        bool recoverable = true);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
