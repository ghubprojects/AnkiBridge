using AnkiBridge.Domain.Enums;
using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Learning;

public interface ILearningEntryRepository : IRepository<LearningEntry, Guid>
{
    Task<LearningEntry?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string headword, 
        PartOfSpeech partOfSpeech, 
        CancellationToken cancellationToken = default);

    Task AddAsync(
        LearningEntry learningEntry, 
        CancellationToken cancellationToken = default);
}