using LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;
using LexiBridge.Domain.SeedWork;
using LexiBridge.Infrastructure.Persistence.Contexts;

namespace LexiBridge.Infrastructure.Persistence.Repositories;

public sealed class LearningItemExportRepository(ApplicationDbContext context) : ILearningItemExportRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task AddAsync(LearningItemExport export, CancellationToken cancellationToken = default)
    {
        await context.LearningItemExports.AddAsync(export, cancellationToken);
    }

    public async Task<LearningItemExport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(LearningItemExport export, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
