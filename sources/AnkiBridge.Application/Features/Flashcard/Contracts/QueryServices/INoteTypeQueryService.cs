using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;

namespace AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;

public interface INoteTypeQueryService
{
    Task<PaginatedResult<NoteTypeSearchResult>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
