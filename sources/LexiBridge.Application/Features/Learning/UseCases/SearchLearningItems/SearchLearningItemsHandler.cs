using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Learning.Abstractions;
using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.SearchLearningItems;

public sealed class SearchLearningItemsHandler(ILearningItemQueryService queryService)
    : IRequestHandler<SearchLearningItemsQuery, Result<PaginatedData<LearningItemSearchResultDTO>>>
{
    public async Task<Result<PaginatedData<LearningItemSearchResultDTO>>> Handle(SearchLearningItemsQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
           request.Keyword,
           request.PageNumber,
           request.PageSize,
           cancellationToken);
    }
}