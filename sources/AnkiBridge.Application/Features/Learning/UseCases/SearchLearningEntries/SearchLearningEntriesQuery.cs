using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Learning.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.SearchLearningEntries;

public sealed record SearchLearningEntriesQuery 
    : PaginationQuery, IRequest<Result<PaginatedData<LearningEntrySearchResultDTO>>>
{
    public string? Keyword { get; init; }
}