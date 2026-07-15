using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Learning;

public interface ILearningEntryRepository : IRepository<LearningEntry, Guid>
{
    Task<IReadOnlyList<LearningEntry>> GetAllAsync(CancellationToken cancellationToken = default);

    void Add(LearningEntry entry);
}
