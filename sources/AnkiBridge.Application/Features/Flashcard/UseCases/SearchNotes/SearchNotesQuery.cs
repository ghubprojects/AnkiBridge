using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SearchNotes;

public sealed record SearchNotesQuery : PaginationQuery, IRequest<Result<PaginatedResult<NoteSearchResult>>>
{
    public string? Headword { get; init; }
    public string? NoteType { get; init; }
    public string? Deck { get; init; }
    public ExportStatus? Status { get; init; }
}