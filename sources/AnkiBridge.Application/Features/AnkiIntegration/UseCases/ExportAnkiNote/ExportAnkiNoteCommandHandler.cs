using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.ExportAnkiNote;

public sealed class ExportAnkiNoteCommandHandler(
    IAnkiNoteRepository noteRepository)
    : IRequestHandler<ExportAnkiNoteCommand, Result>
{
    public async Task<Result> Handle(ExportAnkiNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await noteRepository.GetByIdAsync(
            request.Id,
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