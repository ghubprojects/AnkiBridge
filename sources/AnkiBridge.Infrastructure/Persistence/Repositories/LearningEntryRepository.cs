using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

internal sealed class LearningEntryRepository(ApplicationDbContext context) : ILearningEntryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<IReadOnlyList<LearningEntry>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.LearningEntries
            .Include(entry => entry.Examples)
            .OrderByDescending(entry => entry.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public void Add(LearningEntry entry) => context.LearningEntries.Add(entry);
}
