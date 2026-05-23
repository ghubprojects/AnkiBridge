using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SearchNoteTypes;

public sealed record SearchNoteTypesQuery 
    : PaginationQuery, IRequest<Result<PaginatedResult<NoteTypeSearchResult>>>
{
    public string? Keyword { get; init; }
}