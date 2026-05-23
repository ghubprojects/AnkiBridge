using AnkiBridge.Application.Features.Learning.Contracts.QueryServices.Models;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.UpdateLearningEntry;

public sealed class UpdateLearningEntryHandler(ILearningEntryRepository LearningEntryRepository)
    : IRequestHandler<UpdateLearningEntryCommand, Result>
{
    public async Task<Result> Handle(UpdateLearningEntryCommand request, CancellationToken cancellationToken)
    {
        var item = await LearningEntryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (item is null)
            return Result.Failure<LearningEntryDetail>("Learning item not found.", ErrorType.NotFound);

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

        await LearningEntryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}