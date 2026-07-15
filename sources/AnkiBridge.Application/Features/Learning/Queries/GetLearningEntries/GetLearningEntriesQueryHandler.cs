using AnkiBridge.Domain.Aggregates.Learning;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.Queries.GetLearningEntries;

public sealed class GetLearningEntriesQueryHandler(ILearningEntryRepository repository)
    : IRequestHandler<GetLearningEntriesQuery, IReadOnlyList<LearningEntryListItem>>
{
    public async Task<IReadOnlyList<LearningEntryListItem>> Handle(
        GetLearningEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var entries = await repository.GetAllAsync(cancellationToken);

        return entries
            .Select(entry => new LearningEntryListItem(
                entry.Id,
                entry.Headword,
                entry.PartOfSpeech,
                entry.Cloze,
                entry.Definition,
                entry.Translation,
                entry.Accent,
                entry.Ipa,
                entry.Examples.Count,
                entry.CreatedAt,
                entry.DictionaryEntryId))
            .ToList();
    }
}
