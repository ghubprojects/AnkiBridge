namespace AnkiBridge.Infrastructure.Persistence.Seeding.Models;

internal sealed record NoteTypeSeed
{
    public required string Name { get; init; }
    public long ExternalId { get; init; }
}