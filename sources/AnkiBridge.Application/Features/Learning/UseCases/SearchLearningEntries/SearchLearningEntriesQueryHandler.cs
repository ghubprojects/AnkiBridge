using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Learning.Abstractions;
using AnkiBridge.Application.Features.Learning.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.SearchLearningEntries;

public sealed class SearchLearningEntriesHandler(ILearningEntryQueryService queryService)
    : IRequestHandler<SearchLearningEntriesQuery, Result<PaginatedData<LearningEntrySearchResultDTO>>>
{
    public async Task<Result<PaginatedData<LearningEntrySearchResultDTO>>> Handle(
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