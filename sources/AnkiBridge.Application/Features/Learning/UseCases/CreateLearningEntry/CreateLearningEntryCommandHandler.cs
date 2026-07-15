using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.CreateLearningEntry;

public sealed class CreateLearningEntryCommandHandler(ILearningEntryRepository repository)
    : IRequestHandler<CreateLearningEntryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateLearningEntryCommand command,
        CancellationToken cancellationToken)
    {
        var cloze = string.IsNullOrWhiteSpace(command.Cloze)
            && !string.IsNullOrWhiteSpace(command.Headword)
            ? LearningEntry.GenerateCloze(command.Headword)
            : command.Cloze;

        var entryResult = LearningEntry.Create(
            command.Headword,
            command.PartOfSpeech,
            cloze,
            command.Definition,
            command.Translation,
            command.TranslationSource,
            command.Accent,
            command.Ipa,
            command.DictionaryEntryId,
            command.Examples);

        if (entryResult.IsFailure)
            return Result.Failure<Guid>(entryResult.Error, entryResult.ErrorType);

        repository.Add(entryResult.Value);
        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return entryResult.Value.Id;
    }
}
