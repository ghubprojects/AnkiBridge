using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchAnkiNotes;

public sealed record SearchAnkiNotesQuery : PaginationQuery, IRequest<Result<PaginatedData<AnkiNoteSearchResultDTO>>>
{
    public string? Headword { get; init; }
    public string? NoteType { get; init; }
    public string? Deck { get; init; }
    public ExportStatus? Status { get; init; }
}