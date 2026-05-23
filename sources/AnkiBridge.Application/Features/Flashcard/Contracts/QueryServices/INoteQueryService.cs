using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;

public interface INoteQueryService
{
    Task<PaginatedResult<NoteSearchResult>> SearchAsync(
        string? headword,
        string? noteType,
        string? deck,
        ExportStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}