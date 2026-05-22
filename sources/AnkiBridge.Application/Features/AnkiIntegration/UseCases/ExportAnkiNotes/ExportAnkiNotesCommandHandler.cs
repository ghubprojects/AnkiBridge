using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Application.Abstractions.TransactionalOutbox;
using AnkiBridge.Application.Features.AnkiIntegration.IntegrationEvents;
using AnkiBridge.Shared.Results;
using MediatR;
using System.Text.Json;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.ExportAnkiNotes;

public sealed class ExportAnkiNotesCommandHandler(
    IAnkiNoteRepository noteRepository,
    IOutboxMessageRepository outboxMessageRepository)
    : IRequestHandler<ExportAnkiNotesCommand, Result>
{
    public async Task<Result> Handle(ExportAnkiNotesCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToArray();

        if (ids.Length == 0)
        {
            return Result.ValidationFailure(
                [new ValidationError(nameof(ExportAnkiNotesCommand.Ids), "Ids cannot be empty.")]);
        }

        if (ids.Length != request.Ids.Count)
        {
            return Result.ValidationFailure(
                [new ValidationError(nameof(ExportAnkiNotesCommand.Ids), "Ids must be unique.")]);
        }

        var notes = await noteRepository.GetByIdsAsync(
            ids,
            includeRelated: false,
            cancellationToken);

        var existingIds = notes
            .Select(x => x.Id)
            .ToHashSet();

        var missingIds = ids
            .Where(id => !existingIds.Contains(id))
            .ToArray();

        if (missingIds.Length > 0)
        {
            return Result.Failure(
                $"Anki notes not found: {string.Join(", ", missingIds)}.",
                ErrorType.NotFound);
        }

        foreach (var note in notes)
        {
            var markResult = note.MarkAsProcessingSilently();
            if (markResult.IsFailure)
                return markResult;
        }

        var integrationEvent = new AnkiNotesExportStartedIntegrationEvent(ids);

        var message = new OutboxMessage(
            JsonSerializer.Serialize(integrationEvent),
            integrationEvent.GetType().FullName!);

        await outboxMessageRepository.AddAsync(message, cancellationToken);

        await noteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}