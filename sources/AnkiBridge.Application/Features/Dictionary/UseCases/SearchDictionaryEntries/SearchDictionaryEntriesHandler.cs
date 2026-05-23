using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Dictionary.Contracts.QueryServices;
using AnkiBridge.Application.Features.Dictionary.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.UseCases.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesQueryHandler(
    IDictionaryEntryQueryService queryService)
    : IRequestHandler<SearchDictionaryEntriesQuery, Result<PaginatedResult<DictionaryEntrySearchResult>>>
{
    public async Task<Result<PaginatedResult<DictionaryEntrySearchResult>>> Handle(SearchDictionaryEntriesQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
            request.Keyword,
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);
    }
}