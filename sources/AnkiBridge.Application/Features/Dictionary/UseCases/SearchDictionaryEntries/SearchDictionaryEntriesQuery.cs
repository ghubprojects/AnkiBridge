using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Dictionary.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.UseCases.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesQuery(
    string Keyword
) : PaginationQuery, IRequest<Result<PaginatedData<DictionaryEntrySearchResultDTO>>>;
