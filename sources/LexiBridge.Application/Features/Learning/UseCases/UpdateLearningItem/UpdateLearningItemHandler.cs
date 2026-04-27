using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.UpdateLearningItem;

public sealed class UpdateLearningItemHandler(ILearningItemRepository learningItemRepository)
    : IRequestHandler<UpdateLearningItemCommand, Result>
{
    public async Task<Result> Handle(UpdateLearningItemCommand request, CancellationToken cancellationToken)
    {
        var item = await learningItemRepository.GetByIdAsync(request.Id, cancellationToken);
        if (item is null)
            return Result.Failure<LearningItemDetailDTO>("Learning item not found.", ErrorType.NotFound);

        item.Update(
            request.Headword,
            request.PartOfSpeech,
            request.Ipa,
            request.Accent,
            request.Cloze,
            request.Definition,
            request.Translation,
            request.Examples.Select(x => (x.Id, x.Text)),
            request.AudioUrl,
            request.ImageUrl,
            request.DictionaryEntryId);

        await learningItemRepository.UpdateAsync(item, cancellationToken);
        await learningItemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}