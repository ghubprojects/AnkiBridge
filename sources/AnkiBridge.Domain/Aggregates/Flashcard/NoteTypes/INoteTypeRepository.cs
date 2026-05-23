using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Flashcard.NoteTypes;

public interface INoteTypeRepository : IRepository<NoteType, Guid>
{
    Task<List<NoteType>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<NoteType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
       string name,
       CancellationToken cancellationToken = default);

    Task AddAsync(
        NoteType noteType,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<NoteType> noteTypes,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        NoteType noteType,
        CancellationToken cancellationToken = default);
}
