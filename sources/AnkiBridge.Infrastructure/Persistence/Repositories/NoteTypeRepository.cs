using AnkiBridge.Domain.Aggregates.Flashcard.NoteTypes;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

public sealed class NoteTypeRepository(ApplicationDbContext context) : INoteTypeRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<List<NoteType>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.NoteTypes
            .ToListAsync(cancellationToken);
    }

    public async Task<NoteType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.NoteTypes
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
       string name,
       CancellationToken cancellationToken = default)
    {
        return await context.NoteTypes
            .AnyAsync(x =>
                x.Name == name,
                cancellationToken);
    }

    public async Task AddAsync(
        NoteType noteType,
       CancellationToken cancellationToken = default)
    {
        await context.AddAsync(noteType, cancellationToken);
    }

    public async Task AddRangeAsync(
        IEnumerable<NoteType> noteTypes,
       CancellationToken cancellationToken = default)
    {
        await context.AddRangeAsync(noteTypes, cancellationToken);
    }

    public async Task DeleteAsync(
        NoteType noteType,
       CancellationToken cancellationToken = default)
    {
        context.NoteTypes.Remove(noteType);
    }
}
