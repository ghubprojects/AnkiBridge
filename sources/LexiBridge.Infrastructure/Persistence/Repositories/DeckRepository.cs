using LexiBridge.Domain.Aggregates.Exporting.Decks;
using LexiBridge.Domain.SeedWork;
using LexiBridge.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LexiBridge.Infrastructure.Persistence.Repositories;

public sealed class DeckRepository(ApplicationDbContext context) : IDeckRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<Deck?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Decks
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Deck deck, CancellationToken cancellationToken)
    {
        await context.Decks.AddAsync(deck, cancellationToken);
    }

    public Task UpdateAsync(Deck deck, CancellationToken cancellationToken)
    {
        context.Decks.Update(deck);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Decks.AnyAsync(x => x.Id == id, cancellationToken);
    }
}