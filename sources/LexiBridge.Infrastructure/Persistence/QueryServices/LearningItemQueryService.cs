using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Learning.Abstractions;
using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Infrastructure.Persistence.Contexts;
using LexiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LexiBridge.Infrastructure.Persistence.QueryServices;

public sealed class LearningItemQueryService(
    ApplicationDbContext context)
    : ILearningItemQueryService
{
    public async Task<PaginatedData<LearningItemSearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.LearningItems.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(x => EF.Functions.Like(x.Headword, $"%{keyword}%"));

        return await query.OrderByDescending(x => x.CreatedAt)
            .ToPaginatedDataAsync(
                pageNumber,
                pageSize,
                x => new LearningItemSearchResultDTO(
                    x.Id,
                    x.Headword,
                    x.PartOfSpeech,
                    x.Ipa,
                    x.Translation,
                    x.CreatedAt),
                cancellationToken);
    }
}
