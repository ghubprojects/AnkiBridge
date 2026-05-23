using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Learning.Contracts.QueryServices.Models;

namespace AnkiBridge.Application.Features.Learning.Contracts.QueryServices;

public interface ILearningEntryQueryService
{
    Task<PaginatedResult<LearningEntrySearchResult>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
