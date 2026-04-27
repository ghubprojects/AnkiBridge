using LexiBridge.Domain.Aggregates.Dictionary;
using LexiBridge.Domain.Enums;

namespace LexiBridge.Infrastructure.Persistence.Seeding.DTO;

internal sealed record DictionaryEntrySeedDTO
{
    public required string Headword { get; init; }
    public PartOfSpeech PartOfSpeech { get; init; }
    public EntrySource Source { get; init; }

    public List<DictionaryPronunciationSeedDTO> Pronunciations { get; init; } = [];
    public List<DictionaryDefinitionSeedDTO> Definitions { get; init; } = [];
    public List<DictionaryImageSeedDTO> Images { get; init; } = [];
}

internal sealed record DictionaryPronunciationSeedDTO
{
    public required string Ipa { get; init; }
    public Accent Accent { get; init; }
    public required string AudioUrl { get; init; }
    public AudioSource AudioSource { get; init; }
}

internal sealed record DictionaryDefinitionSeedDTO
{
    public required string Text { get; init; }
    public List<string> Examples { get; init; } = [];
}

internal sealed record DictionaryImageSeedDTO
{
    public required string Url { get; init; }
    public ImageSource Source { get; init; }
}
