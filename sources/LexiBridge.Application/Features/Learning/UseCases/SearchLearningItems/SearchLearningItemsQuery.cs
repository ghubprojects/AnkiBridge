using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.SearchLearningItems;

public sealed record SearchLearningItemsQuery : PaginationQuery, IRequest<Result<PaginatedData<LearningItemSearchResultDTO>>>
{
    public string? Keyword { get; init; }
}