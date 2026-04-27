using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Dictionary.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Dictionary.UseCases.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesQuery(
    string Keyword
) : PaginationQuery, IRequest<Result<PaginatedData<DictionaryEntrySearchResultDTO>>>;
