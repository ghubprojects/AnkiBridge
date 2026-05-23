using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Flashcard.Decks;

public interface IDeckRepository : IRepository<Deck, Guid>
{
    Task<List<Deck>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<Deck?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string deckName,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Deck deck,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<Deck> decks,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Deck deck,
        CancellationToken cancellationToken = default);
}