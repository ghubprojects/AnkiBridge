namespace AnkiBridge.Infrastructure.Persistence.Seeding.DTO;

internal sealed record AnkiNoteSeedDTO
{
    // Resolve by Name to avoid hardcoding GUIDs
    public required string DeckName { get; init; }
    public required string NoteTypeName { get; init; }

    // Optional: if null -> take first LearningEntry from DB
    public Guid? LearningEntryId { get; init; }
}