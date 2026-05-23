using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SearchNoteTypes;

public sealed class SearchNoteTypesQueryHandler(INoteTypeQueryService queryService)
    : IRequestHandler<SearchNoteTypesQuery, Result<PaginatedResult<NoteTypeSearchResult>>>
{
    public async Task<Result<PaginatedResult<NoteTypeSearchResult>>> Handle(SearchNoteTypesQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
           request.Keyword,
           request.PageNumber,
           request.PageSize,
           cancellationToken);
    }
}