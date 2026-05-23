using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.ExportNotes;

public sealed record ExportNotesCommand(IReadOnlyList<Guid> NoteIds) : IRequest<Result>;