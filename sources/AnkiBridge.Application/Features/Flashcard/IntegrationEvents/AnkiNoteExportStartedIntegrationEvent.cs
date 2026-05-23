using AnkiBridge.Application.Common.IntegrationEvents;

namespace AnkiBridge.Application.Features.Flashcard.IntegrationEvents;

public sealed record AnkiNoteExportStartedIntegrationEvent(Guid AnkiNoteId) : IntegrationEvent;
