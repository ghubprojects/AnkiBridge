using AnkiBridge.Application.Abstractions.TransactionalOutbox;
using AnkiBridge.Application.Features.AnkiIntegration.IntegrationEvents;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note.Events;
using MediatR;
using System.Text.Json;

namespace AnkiBridge.Application.Features.AnkiIntegration.DomainEventHandlers;

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