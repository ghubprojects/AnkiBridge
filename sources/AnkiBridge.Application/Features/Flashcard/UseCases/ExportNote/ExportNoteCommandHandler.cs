using AnkiBridge.Domain.Aggregates.Flashcard.Notes;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.ExportNote;

public sealed class ExportNoteCommandHandler(INoteRepository noteRepository)
    : IRequestHandler<ExportNoteCommand, Result>
{
    public async Task<Result> Handle(ExportNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(
            request.NoteId,
            includeRelated: false,
            cancellationToken);

        if (note is null)
            return Result.Failure("Anki note not found.", ErrorType.NotFound);

        var result = note.MarkAsProcessing();
        if (result.IsFailure)
            return result;

        await noteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}