using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Exporting.Decks;

public interface IDeckRepository : IRepository<Deck, Guid>
{
    Task<Deck?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Deck deck,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Deck deck,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}