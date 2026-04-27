using LexiBridge.Application.Abstractions.Services;
using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.CreateLearningItem;

public sealed class CreateLearningItemHandler(
    ILearningItemRepository learningItemRepository,
    IFileStorageService fileStorageService)
    : IRequestHandler<CreateLearningItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateLearningItemCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await learningItemRepository.ExistsAsync(request.Headword, request.PartOfSpeech, cancellationToken);
        if (alreadyExists)
            return Result.Failure<Guid>("A learning item with the same headword and part of speech already exists.");

        string? audioPath = null;
        string? imagePath = null;

        var learningItem = LearningItem.Create(
            request.Headword,
            request.PartOfSpeech,
            request.Ipa,
            request.Accent,
            request.Cloze,
            request.Definition,
            request.Translation,
            request.Examples,
            audioPath,
            imagePath,
            request.DictionaryEntryId
        );

        var basePath = $"learning-items/{learningItem.Id}";

        if (request.AudioStream is not null)
        {
            var audioResult = await fileStorageService.UploadAsync(
                request.AudioStream,
                $"{basePath}/{request.AudioFileName}",
                request.AudioContentType ?? "audio/mpeg",
                cancellationToken);

            if (audioResult.IsFailure)
                return audioResult.ToFailure<Guid>();

            learningItem.SetAudioPath(audioResult.Value);
        }

        if (request.ImageStream is not null)
        {
            var imageResult = await fileStorageService.UploadAsync(
                request.ImageStream,
                $"{basePath}/{request.ImageFileName}",
                request.ImageContentType ?? "image/jpeg",
                cancellationToken);

            if (imageResult.IsFailure)
                return imageResult.ToFailure<Guid>();

            learningItem.SetImagePath(imageResult.Value);
        }

        await learningItemRepository.AddAsync(learningItem, cancellationToken);

        // TODO: Implement Outbox Pattern for LearningItemCreated events
        await learningItemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return learningItem.Id;
    }
}
