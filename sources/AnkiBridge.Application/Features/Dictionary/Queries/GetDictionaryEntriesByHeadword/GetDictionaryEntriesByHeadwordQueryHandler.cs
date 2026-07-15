using AnkiBridge.Domain.Aggregates.Dictionary;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.Queries.GetDictionaryEntriesByHeadword;

public sealed class GetDictionaryEntriesByHeadwordQueryHandler(IDictionaryEntryRepository repository)
    : IRequestHandler<GetDictionaryEntriesByHeadwordQuery, IReadOnlyList<DictionaryEntryCandidate>>
{
    public async Task<IReadOnlyList<DictionaryEntryCandidate>> Handle(
        GetDictionaryEntriesByHeadwordQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Headword))
            return [];

        var entries = await repository.GetByHeadwordAsync(request.Headword, cancellationToken);

        return entries
            .Select(entry => new DictionaryEntryCandidate(
                entry.Id,
                entry.Headword,
                entry.PartOfSpeech,
                entry.Definitions
                    .OrderBy(definition => definition.OrderIndex)
                    .Select(definition => new DictionaryDefinitionCandidate(
                        definition.Text,
                        definition.Examples
                            .OrderBy(example => example.Id)
                            .Select(example => example.Text)
                            .ToList()))
                    .ToList(),
                entry.Translations
                    .Select(translation => new DictionaryTranslationCandidate(
                        translation.Text,
                        translation.Source))
                    .ToList(),
                entry.Pronunciations
                    .Select(pronunciation => new DictionaryPronunciationCandidate(
                        pronunciation.Ipa,
                        pronunciation.Accent,
                        pronunciation.AudioUrl,
                        pronunciation.AudioSource))
                    .ToList(),
                entry.Images
                    .Select(image => new DictionaryImageCandidate(image.Url, image.Source))
                    .ToList()))
            .ToList();
    }
}
