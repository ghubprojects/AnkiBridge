using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.Note.Events;

public sealed record AnkiNoteExportStartedDomainEvent(Guid AnkiNoteId) : DomainEvent;
