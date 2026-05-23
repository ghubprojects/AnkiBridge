using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SearchNotes;

public sealed class SearchNotesQueryHandler(INoteQueryService queryService)
    : IRequestHandler<SearchNotesQuery, Result<PaginatedResult<NoteSearchResult>>>
{
    public async Task<Result<PaginatedResult<NoteSearchResult>>> Handle(SearchNotesQuery request, CancellationToken cancellationToken)
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