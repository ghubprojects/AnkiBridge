using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SearchDecks;

public sealed record SearchDecksQuery 
    : PaginationQuery, IRequest<Result<PaginatedResult<DeckSearchResult>>>
{
    public string? Keyword { get; init; }
}