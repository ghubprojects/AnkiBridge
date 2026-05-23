using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Learning.Contracts.QueryServices;
using AnkiBridge.Application.Features.Learning.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.SearchLearningEntries;

public sealed class SearchLearningEntriesHandler(ILearningEntryQueryService queryService)
    : IRequestHandler<SearchLearningEntriesQuery, Result<PaginatedResult<LearningEntrySearchResult>>>
{
    public async Task<Result<PaginatedResult<LearningEntrySearchResult>>> Handle(
        SearchLearningEntriesQuery request, 
        CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
           request.Keyword,
           request.PageNumber,
           request.PageSize,
           cancellationToken);
    }
}