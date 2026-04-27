using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Dictionary.Abstractions;
using LexiBridge.Application.Features.Dictionary.DTO;
using LexiBridge.Infrastructure.Persistence.Contexts;
using LexiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LexiBridge.Infrastructure.Persistence.QueryServices;

public sealed class DictionaryEntryQueryService(
    ApplicationDbContext context)
    : IDictionaryEntryQueryService
{
    public async Task<PaginatedData<DictionaryEntrySearchResultDTO>> SearchAsync(
        string keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return await context.DictionaryEntries
            .AsNoTracking()
            .Where(x => EF.Functions.Like(x.Headword, $"%{keyword}%"))
            .OrderBy(x => x.Headword)
            .ToPaginatedDataAsync(
                pageNumber,
                pageSize,
                x => new DictionaryEntrySearchResultDTO(
                    x.Id,
                    x.Headword,
                    x.PartOfSpeech),
                cancellationToken);
    }

    public async Task<DictionaryEntryDetailDTO?> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await context.DictionaryEntries
            .Include(e => e.Pronunciations)
            .Include(e => e.Definitions)
                .ThenInclude(d => d.Examples)
            .Include(e => e.Images)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new DictionaryEntryDetailDTO(
                x.Id,
                x.Headword,
                x.PartOfSpeech,
                x.Pronunciations
                    .Select(p => new PronunciationDTO(
                        p.Accent,
                        p.Ipa,
                        p.AudioUrl))
                    .ToList(),
                x.Definitions
                    .Select(d => new DefinitionDTO(
                        d.Text,
                        d.Examples.Select(e => e.Text).ToList()))
                    .ToList(),
                x.Images.Select(i => i.Url).ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
