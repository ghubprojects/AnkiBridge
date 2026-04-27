using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.GetLearningItem;

public sealed class GetLearningItemHandler(
    ILearningItemRepository learningItemRepository)
    : IRequestHandler<GetLearningItemQuery, Result<LearningItemDetailDTO>>
{
    public async Task<Result<LearningItemDetailDTO>> Handle(
        GetLearningItemQuery request,
        CancellationToken cancellationToken)
    {
        var item = await learningItemRepository.GetByIdAsync(request.Id, cancellationToken);
        if (item is null)
            return Result.Failure<LearningItemDetailDTO>("Learning item not found.", ErrorType.NotFound);

        return new LearningItemDetailDTO(
            item.Id,
            item.Headword,
            item.PartOfSpeech,
            item.Accent,
            item.Ipa,
            item.Cloze,
            item.Definition,
            item.Translation,
            item.AudioPath,
            item.ImagePath,
            item.Examples.Select(x => new LearningExampleDTO(x.Id, x.Text)).ToList(),
            item.CreatedAt);
    }
}