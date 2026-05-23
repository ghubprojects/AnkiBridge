using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.ExportNote;

public sealed record ExportNoteCommand(Guid NoteId) : IRequest<Result>;