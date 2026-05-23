using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.CreateNotes;

public sealed record CreateNotesCommand(
    IReadOnlyList<Guid> LearningEntryIds,
    Guid NoteTypeId,
    Guid DeckId
) : IRequest<Result<IReadOnlyList<Guid>>>;