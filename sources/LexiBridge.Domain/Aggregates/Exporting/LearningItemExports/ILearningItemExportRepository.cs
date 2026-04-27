using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;

public interface ILearningItemExportRepository : IRepository<LearningItemExport, Guid>
{
    Task<LearningItemExport?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        LearningItemExport export,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        LearningItemExport export,
        CancellationToken cancellationToken = default);
}