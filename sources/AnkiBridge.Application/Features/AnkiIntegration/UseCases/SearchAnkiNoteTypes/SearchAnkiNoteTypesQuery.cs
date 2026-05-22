using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SearchAnkiNoteTypes;

public sealed record SearchAnkiNoteTypesQuery 
    : PaginationQuery, IRequest<Result<PaginatedData<AnkiNoteTypeSearchResultDTO>>>
{
    public string? Keyword { get; init; }
}