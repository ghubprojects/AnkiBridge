using AnkiBridge.Application.Common.Contracts.Outbox;
using AnkiBridge.Application.Features.Flashcard.IntegrationEvents;
using AnkiBridge.Domain.Aggregates.Flashcard.Notes.Events;
using MediatR;
using System.Text.Json;

namespace AnkiBridge.Application.Features.Flashcard.DomainEventHandlers;

public sealed class AnkiNoteExportStartedDomainEventHandler(
    IOutboxMessageRepository outboxMessageRepository)
    : INotificationHandler<AnkiNoteExportStartedDomainEvent>
{
    public async Task Handle(AnkiNoteExportStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AnkiNoteExportStartedIntegrationEvent(notification.AnkiNoteId);

        var message = new OutboxMessage(
            JsonSerializer.Serialize(integrationEvent),
            integrationEvent.GetType().FullName!);

        await outboxMessageRepository.AddAsync(message, cancellationToken);
    }
}