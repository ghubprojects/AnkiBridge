using System.ComponentModel.DataAnnotations;

namespace AnkiBridge.Web.Features.Anki.Models;

public sealed class AnkiNoteFormViewModel
{
    public IReadOnlyList<AnkiOption> DeckOptions = [];

    public IReadOnlyList<AnkiOption> NoteTypeOptions = [];

    [Required]
    public AnkiOption SelectedDeck = default!;

    [Required]
    public AnkiOption SelectedNoteType = default!;
}
