using AnkiBridge.Domain.Enums;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.Queries.GetDictionaryEntriesByHeadword;

public sealed record GetDictionaryEntriesByHeadwordQuery(string Headword)
    : IRequest<IReadOnlyList<DictionaryEntryCandidate>>;

public sealed record DictionaryEntryCandidate(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    IReadOnlyList<DictionaryDefinitionCandidate> Definitions,
    IReadOnlyList<DictionaryTranslationCandidate> Translations,
    IReadOnlyList<DictionaryPronunciationCandidate> Pronunciations,
    IReadOnlyList<DictionaryImageCandidate> Images);

public sealed record DictionaryDefinitionCandidate(
    string Text,
    IReadOnlyList<string> Examples);

public sealed record DictionaryTranslationCandidate(string Text, TranslationSource Source);

public sealed record DictionaryPronunciationCandidate(
    string Ipa,
    Accent Accent,
    string? AudioUrl,
    AudioSource? AudioSource);

public sealed record DictionaryImageCandidate(string Url, ImageSource Source);
