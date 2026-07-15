using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Abstractions.Dictionary;

public interface IDictionaryEntryProvider
{
    Task<Result<IReadOnlyList<DictionaryEntryResult>>> LookupAsync(
        string headword,
        CancellationToken cancellationToken = default);
}

public sealed record DictionaryEntryResult(
    string Headword,
    PartOfSpeech PartOfSpeech,
    IReadOnlyList<DictionaryDefinitionResult> Definitions,
    IReadOnlyList<DictionaryPronunciationResult> Pronunciations);

public sealed record DictionaryDefinitionResult(
    string Text,
    IReadOnlyList<string> Examples);

public sealed record DictionaryPronunciationResult(
    Accent Accent,
    string Ipa,
    string? AudioUrl);
