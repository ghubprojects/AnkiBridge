using AnkiBridge.Application.Abstractions.IntegrationEvents;

namespace AnkiBridge.Application.Features.AnkiIntegration.IntegrationEvents;

public sealed record AnkiNotesExportStartedIntegrationEvent(IReadOnlyList<Guid> AnkiNoteIds) : IntegrationEvent;
