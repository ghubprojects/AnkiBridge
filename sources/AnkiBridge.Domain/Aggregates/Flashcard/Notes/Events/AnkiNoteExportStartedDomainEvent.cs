using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Flashcard.Notes.Events;

public sealed record AnkiNoteExportStartedDomainEvent(Guid AnkiNoteId) : DomainEvent;
