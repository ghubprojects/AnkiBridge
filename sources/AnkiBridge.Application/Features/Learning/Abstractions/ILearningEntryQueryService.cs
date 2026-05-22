using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Learning.DTO;

namespace AnkiBridge.Application.Features.Learning.Abstractions;

public interface ILearningEntryQueryService
{
    Task<PaginatedData<LearningEntrySearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
