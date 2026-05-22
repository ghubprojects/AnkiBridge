using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.CreateAnkiNotes;

public sealed record CreateAnkiNotesCommand(
    IReadOnlyList<Guid> LearningEntryIds,
    Guid NoteTypeId,
    Guid DeckId
) : IRequest<Result<IReadOnlyList<Guid>>>;