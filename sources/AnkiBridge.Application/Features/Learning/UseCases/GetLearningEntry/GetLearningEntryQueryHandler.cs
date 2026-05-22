using AnkiBridge.Application.Features.Learning.DTO;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.GetLearningEntry;

public sealed class GetLearningEntryQueryHandler(
    ILearningEntryRepository LearningEntryRepository)
    : IRequestHandler<GetLearningEntryQuery, Result<LearningEntryDetailDTO>>
{
    public async Task<Result<LearningEntryDetailDTO>> Handle(
        GetLearningEntryQuery request,
        CancellationToken cancellationToken)
    {
        var item = await LearningEntryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (item is null)
            return Result.Failure<LearningEntryDetailDTO>("Learning item not found.", ErrorType.NotFound);

        return new LearningEntryDetailDTO(
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