using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.Abstractions;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchDecks;

public sealed class SearchAnkiDecksQueryHandler(IAnkiDeckQueryService queryService)
    : IRequestHandler<SearchAnkiDecksQuery, Result<PaginatedData<AnkiDeckSearchResultDTO>>>
{
    public async Task<Result<PaginatedData<AnkiDeckSearchResultDTO>>> Handle(SearchAnkiDecksQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
           request.Keyword,
           request.PageNumber,
           request.PageSize,
           cancellationToken);
    }
}