using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;

namespace AnkiBridge.Application.Features.AnkiIntegration.Abstractions;

public interface IAnkiDeckQueryService
{
    Task<PaginatedData<AnkiDeckSearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
