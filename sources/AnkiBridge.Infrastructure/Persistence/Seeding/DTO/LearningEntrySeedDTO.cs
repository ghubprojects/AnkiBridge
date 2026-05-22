using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.DTO;

internal sealed record LearningEntrySeedDTO
{
    public required string Headword { get; init; }
    public PartOfSpeech PartOfSpeech { get; init; }
    public Accent Accent { get; init; }
    public required string Ipa { get; init; }
    public required string Cloze { get; init; }
    public required string Definition { get; init; }
    public required string Translation { get; init; }

    public List<string> Examples { get; init; } = [];

    public string? AudioPath { get; init; }
    public string? ImagePath { get; init; }
    public Guid? DictionaryEntryId { get; init; }
}
