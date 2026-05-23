using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;

namespace AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;

public interface IDeckQueryService
{
    Task<PaginatedResult<DeckSearchResult>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
