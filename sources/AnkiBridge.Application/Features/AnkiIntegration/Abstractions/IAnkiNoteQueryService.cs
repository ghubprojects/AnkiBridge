using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.AnkiIntegration.Abstractions;

public interface IAnkiNoteQueryService
{
    Task<PaginatedData<AnkiNoteSearchResultDTO>> SearchAsync(
        string? headword,
        string? noteType,
        string? deck,
        ExportStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}