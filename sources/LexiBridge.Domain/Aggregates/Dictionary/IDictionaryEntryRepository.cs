using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Dictionary;

public interface IDictionaryEntryRepository : IRepository<DictionaryEntry, Guid>
{
    Task<DictionaryEntry?> FindByHeadwordAsync(
        string headword,
        PartOfSpeech partOfSpeech,
        CancellationToken cancellationToken = default);
}