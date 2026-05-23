using AnkiBridge.Application.Common.Contracts.Storage;
using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.CreateLearningEntry;

public sealed class CreateLearningEntryCommandHandler(
    ILearningEntryRepository learningEntryRepository,
    IFileStorage fileStorageService)
    : IRequestHandler<CreateLearningEntryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateLearningEntryCommand request, CancellationToken cancellationToken)
    {
        var exists = await learningEntryRepository.ExistsAsync(request.Headword, request.PartOfSpeech, cancellationToken);
        if (exists)
            return Result.Failure<Guid>("A learning entry with the same headword and part of speech already exists.");

        var learningEntry = LearningEntry.Create(
            request.Headword,
            request.PartOfSpeech,
            request.Ipa,
            request.Accent,
            request.Cloze,
            request.Definition,
            request.Translation,
            request.Examples,
            null,
            null,
            request.DictionaryEntryId
        );

        var basePath = $"learning-entries/{learningEntry.Id}";

        if (request.AudioStream is not null)
        {
            var audioResult = await fileStorageService.UploadAsync(
                request.AudioStream,
                request.AudioFileName,
                request.AudioContentType ?? "audio/mpeg",
                cancellationToken);

            if (audioResult.IsFailure)
                return audioResult.ToFailure<Guid>();
        }

        if (request.ImageStream is not null)
        {
            var imageResult = await fileStorageService.UploadAsync(
                request.ImageStream,
                request.ImageFileName,
                request.ImageContentType ?? "image/jpeg",
                cancellationToken);

            if (imageResult.IsFailure)
                return imageResult.ToFailure<Guid>();
        }

        await learningEntryRepository.AddAsync(learningEntry, cancellationToken);

        // TODO: Implement Outbox Pattern for learningEntryCreated events
        await learningEntryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return learningEntry.Id;
    }
}
