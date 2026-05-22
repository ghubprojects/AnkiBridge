using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Learning.Abstractions;
using AnkiBridge.Application.Features.Learning.DTO;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.QueryServices;

public sealed class LearningEntryQueryService(
    ApplicationDbContext context)
    : ILearningEntryQueryService
{
    public async Task<PaginatedData<LearningEntrySearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.LearningEntries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(x => EF.Functions.Like(x.Headword, $"%{keyword}%"));

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToPaginatedDataAsync(
                pageNumber,
                pageSize,
                x => new LearningEntrySearchResultDTO(
                    x.Id,
                    x.Headword,
                    x.PartOfSpeech,
                    x.Ipa,
                    x.Translation,
                    x.CreatedAt),
                cancellationToken);
    }
}
