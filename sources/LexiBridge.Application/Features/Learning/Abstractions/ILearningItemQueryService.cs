using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Learning.DTO;

namespace LexiBridge.Application.Features.Learning.Abstractions;

public interface ILearningItemQueryService
{
    Task<PaginatedData<LearningItemSearchResultDTO>> SearchAsync(
        string? keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
