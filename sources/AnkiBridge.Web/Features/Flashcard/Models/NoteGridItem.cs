using AnkiBridge.Domain.Enums;
using AnkiBridge.Web.Components.Abstractions;

namespace AnkiBridge.Web.Features.Flashcard.Models;

public sealed class NoteGridItem : IGridItem<Guid>
{
    public Guid Id { get; init; }
    public string Headword { get; init; } = default!;
    public string Deck { get; init; } = default!;
    public string NoteType { get; init; } = default!;
    public ExportStatus ExportStatus { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
}
