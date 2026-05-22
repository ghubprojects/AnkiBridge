namespace AnkiBridge.Application.Abstractions.IntegrationEvents;

public record IntegrationEvent
{
    public Guid Id { get; }

    public DateTime CreationDate { get; }

    public IntegrationEvent()
    {
        Id = Guid.CreateVersion7();
        CreationDate = DateTime.UtcNow;
    }
}
