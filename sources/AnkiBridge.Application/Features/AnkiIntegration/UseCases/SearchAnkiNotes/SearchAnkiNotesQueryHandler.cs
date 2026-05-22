using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.Abstractions;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchAnkiNotes;

public sealed class SearchAnkiNotesQueryHandler(IAnkiNoteQueryService queryService)
    : IRequestHandler<SearchAnkiNotesQuery, Result<PaginatedData<AnkiNoteSearchResultDTO>>>
{
    public async Task<Result<PaginatedData<AnkiNoteSearchResultDTO>>> Handle(SearchAnkiNotesQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
            request.Headword,
            request.NoteType,
            request.Deck,
            request.Status,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}