using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;

public interface IAnkiNoteRepository : IRepository<AnkiNote, Guid>
{
    Task<AnkiNote?> GetByIdAsync(
        Guid id,
        bool includeRelated = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnkiNote>> GetByIdsAsync(
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
        AnkiNote note,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<AnkiNote> notes, 
        CancellationToken cancellationToken = default);
}