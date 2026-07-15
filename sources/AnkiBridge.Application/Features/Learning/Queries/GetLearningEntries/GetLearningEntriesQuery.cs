using AnkiBridge.Domain.Enums;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.Queries.GetLearningEntries;

public sealed record GetLearningEntriesQuery : IRequest<IReadOnlyList<LearningEntryListItem>>;

public sealed record LearningEntryListItem(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    string Cloze,
    string Definition,
    string Translation,
    Accent Accent,
    string Ipa,
    int ExampleCount,
    DateTimeOffset CreatedAt,
    Guid? DictionaryEntryId);
