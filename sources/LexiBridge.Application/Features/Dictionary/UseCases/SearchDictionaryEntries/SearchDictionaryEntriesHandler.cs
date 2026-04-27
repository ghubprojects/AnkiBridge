using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Dictionary.Abstractions;
using LexiBridge.Application.Features.Dictionary.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Dictionary.UseCases.SearchDictionaryEntries;

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