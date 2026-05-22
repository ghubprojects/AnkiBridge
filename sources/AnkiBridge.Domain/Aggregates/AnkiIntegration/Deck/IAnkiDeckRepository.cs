using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.Deck;

public interface IAnkiDeckRepository : IRepository<AnkiDeck, Guid>
{
    Task<List<AnkiDeck>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<AnkiDeck?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string deckName,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        AnkiDeck deck,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<AnkiDeck> decks,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        AnkiDeck deck,
        CancellationToken cancellationToken = default);
}