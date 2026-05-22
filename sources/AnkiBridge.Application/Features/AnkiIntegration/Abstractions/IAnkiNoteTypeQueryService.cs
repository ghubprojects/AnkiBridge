using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;

namespace AnkiBridge.Application.Features.AnkiIntegration.Abstractions;

public interface IAnkiNoteTypeQueryService
{
    Task<PaginatedData<AnkiNoteTypeSearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
