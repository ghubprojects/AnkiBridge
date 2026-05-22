using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.Abstractions;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.QueryServices;

public sealed class AnkiDeckQueryService(
    ApplicationDbContext context)
    : IAnkiDeckQueryService
{
    public async Task<PaginatedData<AnkiDeckSearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.AnkiDecks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToPaginatedDataAsync(
                pageNumber,
                pageSize,
                x => new AnkiDeckSearchResultDTO(
                    x.Id,
                    x.Name),
                cancellationToken);
    }
}
