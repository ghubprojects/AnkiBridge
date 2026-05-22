using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

public sealed class LearningEntryRepository(
    ApplicationDbContext context) 
    : ILearningEntryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<LearningEntry?> GetByIdAsync(
      Guid id,
      CancellationToken cancellationToken = default)
    {
        return await context.LearningEntries
            .Include(x => x.Examples)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string headword,
        PartOfSpeech partOfSpeech,
        CancellationToken cancellationToken = default)
    {
        return await context.LearningEntries
            .AnyAsync(x =>
                x.Headword == headword &&
                x.PartOfSpeech == partOfSpeech,
                cancellationToken);
    }

    public async Task AddAsync(
        LearningEntry learningEntry, 
        CancellationToken cancellationToken = default)
    {
        await context.LearningEntries.AddAsync(learningEntry, cancellationToken);
    }
}
