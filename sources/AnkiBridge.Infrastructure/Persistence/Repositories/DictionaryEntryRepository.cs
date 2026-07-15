using AnkiBridge.Domain.Aggregates.Dictionary;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiBridge.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IDictionaryEntryRepository bằng ApplicationDbContext.
/// DictionaryEntries là DbSet `internal` trên ApplicationDbContext nên repository này
/// phải nằm trong cùng assembly AnkiBridge.Infrastructure.
/// </summary>
internal sealed class DictionaryEntryRepository(ApplicationDbContext context) : IDictionaryEntryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<IReadOnlyList<DictionaryEntry>> GetByHeadwordAsync(
        string headword,
        CancellationToken cancellationToken = default)
    {
        return await context.DictionaryEntries
            .Include(e => e.Definitions).ThenInclude(d => d.Examples)
            .Include(e => e.Translations)
            .Include(e => e.Pronunciations)
            .Include(e => e.Images)
            .AsSplitQuery()
            .Where(e => EF.Functions.ILike(e.Headword, headword))
            .ToListAsync(cancellationToken);
    }
 
    public async Task<DictionaryEntry?> GetByHeadwordAndPartOfSpeechAsync(
        string headword,
        PartOfSpeech partOfSpeech,
        CancellationToken cancellationToken = default)
    {
        return await context.DictionaryEntries
            .Include(e => e.Definitions).ThenInclude(d => d.Examples)
            .Include(e => e.Translations)
            .Include(e => e.Pronunciations)
            .Include(e => e.Images)
            .AsSplitQuery()
            .FirstOrDefaultAsync(
                e => EF.Functions.ILike(e.Headword, headword) && e.PartOfSpeech == partOfSpeech,
                cancellationToken);
    }
 
    public void Add(DictionaryEntry entry) => context.DictionaryEntries.Add(entry);
}
