using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;
using LexiBridge.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LexiBridge.Infrastructure.Persistence.Repositories;

public sealed class LearningItemRepository(ApplicationDbContext context) : ILearningItemRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task<bool> ExistsAsync(
        string headword,
        PartOfSpeech partOfSpeech,
        CancellationToken cancellationToken = default)
    {
        return context.LearningItems
            .AnyAsync(x =>
                x.Headword == headword &&
                x.PartOfSpeech == partOfSpeech,
                cancellationToken);
    }

    public async Task AddAsync(
        LearningItem learningItem,
        CancellationToken cancellationToken = default)
    {
        await context.LearningItems.AddAsync(learningItem, cancellationToken);
    }

    public Task<LearningItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return context.LearningItems
            .Include(x => x.Examples)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task UpdateAsync(
        LearningItem learningItem,
        CancellationToken cancellationToken = default)
    {
        context.LearningItems.Update(learningItem);
        return Task.CompletedTask;
    }
}
