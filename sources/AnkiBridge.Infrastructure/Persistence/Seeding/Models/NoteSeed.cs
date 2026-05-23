namespace AnkiBridge.Infrastructure.Persistence.Seeding.Models;

internal sealed record NoteSeed
{
    // Resolve by Name to avoid hardcoding GUIDs
    public required string DeckName { get; init; }
    public required string NoteTypeName { get; init; }

    // Optional: if null -> take first LearningEntry from DB
    public Guid? LearningEntryId { get; init; }
}