using System.ComponentModel.DataAnnotations;

namespace AnkiBridge.Web.Features.Flashcard.Models;

public sealed class NoteFormModel
{
    public IReadOnlyList<AnkiOption> DeckOptions = [];

    public IReadOnlyList<AnkiOption> NoteTypeOptions = [];

    [Required]
    public AnkiOption SelectedDeck = default!;

    [Required]
    public AnkiOption SelectedNoteType = default!;
}
