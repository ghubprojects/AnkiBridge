using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.Abstractions;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchAnkiNoteTypes;

public sealed class SearchAnkiNoteTypesQueryHandler(IAnkiNoteTypeQueryService queryService)
    : IRequestHandler<SearchAnkiNoteTypesQuery, Result<PaginatedData<AnkiNoteTypeSearchResultDTO>>>
{
    public async Task<Result<PaginatedData<AnkiNoteTypeSearchResultDTO>>> Handle(SearchAnkiNoteTypesQuery request, CancellationToken cancellationToken)
    {
        return await queryService.SearchAsync(
           request.Keyword,
           request.PageNumber,
           request.PageSize,
           cancellationToken);
    }
}