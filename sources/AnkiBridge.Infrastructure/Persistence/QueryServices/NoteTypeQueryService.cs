using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.QueryServices;

public sealed class NoteTypeQueryService(ApplicationDbContext context) : INoteTypeQueryService
{
    public async Task<PaginatedResult<NoteTypeSearchResult>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.NoteTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenBy(x => x.Name)
            .ToPaginatedResultAsync(
                pageNumber,
                pageSize,
                x => new NoteTypeSearchResult(x.Id, x.Name),
                cancellationToken);
    }
}
