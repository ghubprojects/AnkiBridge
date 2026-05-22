using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;

public interface IAnkiNoteTypeRepository : IRepository<AnkiNoteType, Guid>
{
    Task<List<AnkiNoteType>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<AnkiNoteType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
       string name,
       CancellationToken cancellationToken = default);

    Task AddAsync(
        AnkiNoteType noteType,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<AnkiNoteType> noteTypes,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        AnkiNoteType noteType,
        CancellationToken cancellationToken = default);
}
