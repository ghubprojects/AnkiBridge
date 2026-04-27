using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Learning;

public interface ILearningItemRepository : IRepository<LearningItem, Guid>
{
    Task<LearningItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        LearningItem learningItem,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        LearningItem learningItem,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string headword, 
        PartOfSpeech partOfSpeech, 
        CancellationToken cancellationToken = default);
}