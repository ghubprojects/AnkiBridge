using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

public sealed class AnkiNoteRepository(
    ApplicationDbContext context)
    : IAnkiNoteRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<AnkiNote?> GetByIdAsync(
        Guid id,
        bool includeRelated = false,
        CancellationToken cancellationToken = default)
    {
        var query = context.AnkiNotes.AsQueryable();

        if (includeRelated)
        {
            query = query
                .Include(x => x.LearningEntry)
                .Include(x => x.Deck)
                .Include(x => x.NoteType)
                .AsSplitQuery();
        }

        return await query
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AnkiNote>> GetByIdsAsync(
        IReadOnlyList<Guid> ids,
        bool includeRelated = false,
        CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
            return [];

        var query = context.AnkiNotes
            .Where(x => ids.Contains(x.Id))
            .AsQueryable();

        if (includeRelated)
        {
            query = query
                .Include(x => x.LearningEntry)
                .Include(x => x.Deck)
                .Include(x => x.NoteType)
                .AsSplitQuery();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiNotes
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid learningEntryId,
        Guid noteTypeId,
        Guid deckId,
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiNotes
            .AnyAsync(x =>
                x.LearningEntryId == learningEntryId &&
                x.NoteTypeId == noteTypeId &&
                x.DeckId == deckId,
                cancellationToken);
    }

    public async Task AddAsync(
        AnkiNote note,
        CancellationToken cancellationToken = default)
    {
        context.AnkiNotes.Add(note);
    }

    public async Task AddRangeAsync(
        IEnumerable<AnkiNote> notes,
        CancellationToken cancellationToken = default)
    {
        context.AnkiNotes.AddRange(notes);
    }
}
