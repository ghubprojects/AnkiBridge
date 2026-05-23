using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Learning.Contracts.QueryServices.Models;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.SearchLearningEntries;

public sealed record SearchLearningEntriesQuery 
    : PaginationQuery, IRequest<Result<PaginatedResult<LearningEntrySearchResult>>>
{
    public string? Keyword { get; init; }
}