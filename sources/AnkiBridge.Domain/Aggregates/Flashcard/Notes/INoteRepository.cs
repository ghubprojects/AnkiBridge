using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Flashcard.Notes;

public interface INoteRepository : IRepository<Note, Guid>
{
    Task<Note?> GetByIdAsync(
        Guid id,
        bool includeRelated = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Note>> GetByIdsAsync(
        IReadOnlyList<Guid> ids,
        bool includeRelated = false,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid learningEntryId,
        Guid noteTypeId,
        Guid deckId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Note note,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<Note> notes, 
        CancellationToken cancellationToken = default);
}