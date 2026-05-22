using AnkiBridge.Domain.Aggregates.AnkiIntegration.Deck;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

public sealed class AnkiDeckRepository(
    ApplicationDbContext context) 
    : IAnkiDeckRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<List<AnkiDeck>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiDecks
            .ToListAsync(cancellationToken);
    }

    public async Task<AnkiDeck?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiDecks
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string name, 
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiDecks
            .AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task AddAsync(
        AnkiDeck deck, 
        CancellationToken cancellationToken = default)
    {
        context.AnkiDecks.Add(deck);
    }   

    public async Task AddRangeAsync(
        IEnumerable<AnkiDeck> decks,
        CancellationToken cancellationToken = default)
    {
        context.AnkiDecks.AddRange(decks);
    }

    public async Task DeleteAsync(
        AnkiDeck deck,
        CancellationToken cancellationToken = default)
    {
        context.AnkiDecks.Remove(deck);
    }
}