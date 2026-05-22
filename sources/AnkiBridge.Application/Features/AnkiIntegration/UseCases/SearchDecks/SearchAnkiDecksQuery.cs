using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchDecks;

public sealed record SearchAnkiDecksQuery 
    : PaginationQuery, IRequest<Result<PaginatedData<AnkiDeckSearchResultDTO>>>
{
    public string? Keyword { get; init; }
}