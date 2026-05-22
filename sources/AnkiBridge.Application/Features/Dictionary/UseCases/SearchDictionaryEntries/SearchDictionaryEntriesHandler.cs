using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Dictionary.Abstractions;
using AnkiBridge.Application.Features.Dictionary.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.UseCases.SearchDictionaryEntries;

public sealed class SearchDictionaryEntriesQueryHandler(
    IDictionaryEntryQueryService queryService)
    : IRequestHandler<SearchDictionaryEntriesQuery, Result<PaginatedData<DictionaryEntrySearchResultDTO>>>
{
    public async Task<Result<PaginatedData<DictionaryEntrySearchResultDTO>>> Handle(SearchDictionaryEntriesQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
            request.Keyword,
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);
    }
}