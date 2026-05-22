using AnkiBridge.Domain.Enums;
using AnkiBridge.Web.Components.Abstractions;

namespace AnkiBridge.Web.Features.Anki.Models;

public sealed class AnkiNoteGridItemViewModel : IGridItem<Guid>
{
    public Guid Id { get; init; }
    public Guid LearningEntryId { get; init; }
    public string LearningEntryHeadword { get; init; } = default!;
    public Guid NoteTypeId { get; init; }
    public string NoteTypeName { get; init; } = default!;
    public Guid DeckId { get; init; }
    public string DeckName { get; init; } = default!;
    public ExportStatus ExportStatus { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}
