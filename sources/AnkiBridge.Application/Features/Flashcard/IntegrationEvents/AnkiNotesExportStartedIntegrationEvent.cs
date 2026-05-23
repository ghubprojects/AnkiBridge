using AnkiBridge.Application.Common.IntegrationEvents;

namespace AnkiBridge.Application.Features.Flashcard.IntegrationEvents;

public sealed record AnkiNotesExportStartedIntegrationEvent(IReadOnlyList<Guid> AnkiNoteIds) : IntegrationEvent;
