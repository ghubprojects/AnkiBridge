using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SearchDecks;

public sealed class SearchDecksQueryHandler(IDeckQueryService queryService)
    : IRequestHandler<SearchDecksQuery, Result<PaginatedResult<DeckSearchResult>>>
{
    public async Task<Result<PaginatedResult<DeckSearchResult>>> Handle(SearchDecksQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
           request.Keyword,
           request.PageNumber,
           request.PageSize,
           cancellationToken);
    }
}