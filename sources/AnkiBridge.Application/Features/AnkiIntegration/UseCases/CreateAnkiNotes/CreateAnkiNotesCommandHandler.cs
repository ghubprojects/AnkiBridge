using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.CreateAnkiNotes;

public sealed class CreateAnkiNotesCommandHandler(
    IAnkiNoteRepository ankiNoteRepository)
    : IRequestHandler<CreateAnkiNotesCommand, Result<IReadOnlyList<Guid>>>
{
    public async Task<Result<IReadOnlyList<Guid>>> Handle(
        CreateAnkiNotesCommand request,
        CancellationToken cancellationToken)
    {
        var learningEntryIds = request.LearningEntryIds.Distinct().ToArray();

        if (learningEntryIds.Length != request.LearningEntryIds.Count)
        {
            return Result.ValidationFailure<IReadOnlyList<Guid>>(
                [new ValidationError(nameof(CreateAnkiNotesCommand.LearningEntryIds), "LearningEntryIds must be unique.")]);
        }

        List<Guid> existingLearningEntryIds = [];

        foreach (var learningEntryId in learningEntryIds)
        {
            var exists = await ankiNoteRepository.ExistsAsync(
                learningEntryId,
                request.NoteTypeId,
                request.DeckId,
                cancellationToken);

            if (exists)
                existingLearningEntryIds.Add(learningEntryId);
        }

        if (existingLearningEntryIds.Count > 0)
        {
            return Result.Failure<IReadOnlyList<Guid>>(
                $"Anki note already exists for LearningEntryIds: {string.Join(", ", existingLearningEntryIds)}.",
                ErrorType.Conflict);
        }

        var notes = learningEntryIds
            .Select(learningEntryId => AnkiNote.Create(
                learningEntryId,
                request.NoteTypeId,
                request.DeckId))
            .ToList();

        await ankiNoteRepository.AddRangeAsync(notes, cancellationToken);

        await ankiNoteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result<IReadOnlyList<Guid>>.Success(learningEntryIds);
    }
}