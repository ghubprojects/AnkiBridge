using AnkiBridge.Domain.Aggregates.Flashcard.Decks;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

public sealed class DeckRepository(ApplicationDbContext context) : IDeckRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<List<Deck>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Decks
            .ToListAsync(cancellationToken);
    }

    public async Task<Deck?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.Decks
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await context.Decks
            .AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task AddAsync(
        Deck deck,
        CancellationToken cancellationToken = default)
    {
        context.Decks.Add(deck);
    }

    public async Task AddRangeAsync(
        IEnumerable<Deck> decks,
        CancellationToken cancellationToken = default)
    {
        context.Decks.AddRange(decks);
    }

    public async Task DeleteAsync(
        Deck deck,
        CancellationToken cancellationToken = default)
    {
        context.Decks.Remove(deck);
    }
}