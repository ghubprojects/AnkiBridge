using AnkiBridge.Domain.Aggregates.Flashcard.Notes;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.CreateNotes;

public sealed class CreateNotesCommandHandler(INoteRepository ankiNoteRepository)
    : IRequestHandler<CreateNotesCommand, Result<IReadOnlyList<Guid>>>
{
    public async Task<Result<IReadOnlyList<Guid>>> Handle(
        CreateNotesCommand request,
        CancellationToken cancellationToken)
    {
        var learningEntryIds = request.LearningEntryIds.Distinct().ToArray();

        if (learningEntryIds.Length != request.LearningEntryIds.Count)
        {
            return Result.ValidationFailure<IReadOnlyList<Guid>>(
                [new ValidationError(nameof(CreateNotesCommand.LearningEntryIds), "LearningEntryIds must be unique.")]);
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
            .Select(learningEntryId => Note.Create(
                learningEntryId,
                request.NoteTypeId,
                request.DeckId))
            .ToList();

        await ankiNoteRepository.AddRangeAsync(notes, cancellationToken);

        await ankiNoteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result<IReadOnlyList<Guid>>.Success(learningEntryIds);
    }
}