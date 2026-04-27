using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.GetLearningItem;

public sealed record GetLearningItemQuery(
    Guid Id
) : IRequest<Result<LearningItemDetailDTO>>;