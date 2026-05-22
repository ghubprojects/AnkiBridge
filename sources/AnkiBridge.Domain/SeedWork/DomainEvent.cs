namespace AnkiBridge.Domain.SeedWork;

/// <summary>
/// Base implementation for domain events
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.CreateVersion7();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
    public int Version => 1;
}
