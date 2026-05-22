using AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

public sealed class AnkiNoteTypeRepository(
    ApplicationDbContext context) 
    : IAnkiNoteTypeRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<List<AnkiNoteType>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiNoteTypes
            .ToListAsync(cancellationToken);
    }

    public async Task<AnkiNoteType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.AnkiNoteTypes
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
       string name,
       CancellationToken cancellationToken = default)
    {
        return await context.AnkiNoteTypes
            .AnyAsync(x =>
                x.Name == name,
                cancellationToken);
    }

    public async Task AddAsync(
        AnkiNoteType noteType,
       CancellationToken cancellationToken = default)
    {
        await context.AddAsync(noteType, cancellationToken);
    }

    public async Task AddRangeAsync(
        IEnumerable<AnkiNoteType> noteTypes,
       CancellationToken cancellationToken = default)
    {
        await context.AddRangeAsync(noteTypes, cancellationToken);
    }

    public async Task DeleteAsync(
        AnkiNoteType noteType,
       CancellationToken cancellationToken = default)
    {
        context.AnkiNoteTypes.Remove(noteType);
    }
}
