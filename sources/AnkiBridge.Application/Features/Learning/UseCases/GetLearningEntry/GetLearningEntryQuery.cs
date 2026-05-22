using AnkiBridge.Application.Features.Learning.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.GetLearningEntry;

public sealed record GetLearningEntryQuery(
    Guid Id
) : IRequest<Result<LearningEntryDetailDTO>>;