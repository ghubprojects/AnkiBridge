using AnkiBridge.Domain.Enums;
using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Dictionary;

public interface IDictionaryEntryRepository : IRepository<DictionaryEntry, Guid>
{
    Task<IReadOnlyList<DictionaryEntry>> GetByHeadwordAsync(
        string headword,
        CancellationToken cancellationToken = default);

    Task<DictionaryEntry?> GetByHeadwordAndPartOfSpeechAsync(
        string headword,
        PartOfSpeech partOfSpeech,
        CancellationToken cancellationToken = default);

    void Add(DictionaryEntry entry);
}